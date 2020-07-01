/*
 * *******************************************************
 *
 * 作者：hzy
 *
 * 开源地址：https://gitee.com/hzy6
 *
 * *******************************************************
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HZY.Admin
{
    using Microsoft.OpenApi.Models;
    using System.IO;
    using Swashbuckle.AspNetCore.Filters;
    using Microsoft.EntityFrameworkCore;
    using HZY.EFCore.Repository;
    using HZY.EFCore;
    using HZY.Admin.Core;
    using HZY.Toolkit;
    using HZY.Toolkit.Entitys;
    using HZY.Admin.Hubs;
    using UEditor.Core;
    using Microsoft.Extensions.Logging;
    using Microsoft.AspNetCore.Mvc;
    using HZY.Admin.Services.Core;
    using log4net;
    using log4net.Repository;
    using log4net.Config;
    using HZY.Admin.Dapper;
    using HZY.DapperCore.Dapper;

    public class Startup
    {
        public static ILoggerRepository repository { get; set; }
        private static readonly IEnumerable<string> _VersionList = typeof(ApiVersionsEnum).GetEnumNames().ToList().OrderBy(w => w);

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;



        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }

        public static readonly ILoggerFactory efLogger = LoggerFactory.Create(builder => { builder.AddConsole(); });

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region HttpContext
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            #endregion

            #region AppConfigs
            //配置 AppConfigs
            AppConfig.AdminConfig = Configuration.GetSection("AppConfig").Get<AdminConfig>();
            AppConfig.LDAP = Configuration.GetSection("LDAP").Get<LDAP>();
            //AppConfig.WebPath = Configuration.GetSection("WebPath").Get<WebUrl>();
            #endregion

            //连接mysql
            services.AddDapper("MSSQL1", m =>
            {
                m.ConnectionString = Configuration.GetSection("DapperConnectionStrings").Get<MSSQL>().MSSQL1;// ["DapperConnectionStrings:MSSQL1"];
                m.DbType = DbStoreType.SqlServer;
            });



            #region 注入 EFCore

            services.AddDbContext<EFCoreContext>(options =>
            {
                options
                .UseSqlServer(Configuration.GetConnectionString(nameof(EFCoreContext)))
                .UseLoggerFactory(efLogger)
                ;
                //无跟踪  , b => b.MigrationsAssembly("HZY.Models")
                // options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            //services.AddIdentity<ApplicationUser, IdentityRole>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>()
            //    .AddDefaultTokenProviders();

            services.AddScoped(typeof(DefaultRepository<>));
            // services.AddScoped(typeof(IRepository<,>), typeof(DefaultRepository<>));
            #endregion

            #region 注入 业务 服务
            services.ServiceStart(typeof(Startup));
            #endregion

            #region 跨域配置 配置跨域处理
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("HZYCors", builder =>
            //    {
            //        builder.WithOrigins("*")
            //            .AllowAnyMethod()
            //            .AllowAnyHeader();
            //        //.AllowAnyOrigin()
            //        //.AllowCredentials();
            //    });
            //});
            #endregion

            #region JWT
            services.AddAuthentication(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,//是否验证Issuer
                        ValidateAudience = true,//是否验证Audience
                        ValidateLifetime = true,//是否验证失效时间
                        ValidateIssuerSigningKey = true,//是否验证SecurityKey
                        ValidAudience = AppConfig.AdminConfig.JwtKeyName,//Audience
                        ValidIssuer = AppConfig.AdminConfig.JwtKeyName,//Issuer，这两项和前面签发jwt的设置一致
                        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(AppConfig.AdminConfig.JwtSecurityKey))//拿到SecurityKey
                    };

                    #region SignalR 配置 jwt
                    //options.Events = new JwtBearerEvents
                    //{
                    //    OnMessageReceived = context =>
                    //    {
                    //        var token = AccountService.GetToken(context.HttpContext);

                    //        // If the request is for our hub...
                    //        var path = context.HttpContext.Request.Path;

                    //        if (!string.IsNullOrEmpty(token) && path.StartsWithSegments("/chatHub"))
                    //        {
                    //            // Read the token out of the query string
                    //            context.Token = token;
                    //        }

                    //        return Task.CompletedTask;
                    //    }
                    //};
                    #endregion

                });
            #endregion

            #region Swagger 注册Swagger生成器，定义一个和多个Swagger 文档
            services.AddSwaggerGen(options =>
            {
                foreach (var item in _VersionList)
                {
                    options.SwaggerDoc(item, new OpenApiInfo { Title = "HZY.Admin" });
                }
                //为 Swagger JSON and UI设置xml文档注释路径
                var xmlPath = Path.Combine(System.AppContext.BaseDirectory, "HZY.Admin.xml");
                var xmlPath1 = Path.Combine(System.AppContext.BaseDirectory, "HZY.Models.xml");
                options.IncludeXmlComments(xmlPath, true);
                options.IncludeXmlComments(xmlPath1, true);

                #region Jwt token 配置
                //option.OperationFilter<AppService.SwaggerParameterFilter>(); // 给每个接口配置授权码传入参数文本框
                //
                options.OperationFilter<AddResponseHeadersFilter>();
                options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                //很重要！这里配置安全校验，和之前的版本不一样
                options.OperationFilter<SecurityRequirementsOperationFilter>();
                //开启 oauth2 安全描述
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    //Scheme = "basic",
                });

                #endregion

            });
            #endregion

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add<HZY.Admin.Core.HZYAppExceptionFilter>();
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
                options.MaxModelValidationErrors = 50;
            })
            .AddNewtonsoftJson(options =>
            {
                //忽略循环引用
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                //不使用驼峰样式的key
                options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                //设置时间格式
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });

            #region 中间件 注入
            services.AddTransient<HZYAppMiddleware>();
            #endregion

            #region 自定义视图
            //自定义 视图 
            services.Configure<Microsoft.AspNetCore.Mvc.Razor.RazorViewEngineOptions>(item =>
            {
                //系统管理
                item.ViewLocationFormats.Add("/Views/Sys/{1}/{0}.cshtml");
                //基础管理
                item.ViewLocationFormats.Add("/Views/Base/{1}/{0}.cshtml");

                item.ViewLocationFormats.Add("/Views/Activity/{1}/{0}.cshtml");
            });
            #endregion

            #region SignalR
            services.AddSignalR();
            #endregion

            #region Ueditor
            //Ueditor  编辑器 服务端 注入  configFileRelativePath: "wwwroot/admin/libs/nUeditor/net/config.json", isCacheConfig: false, basePath: "C:/basepath"
            services.AddUEditorService(
                    configFileRelativePath: WebHostEnvironment.WebRootPath + "/admin/libs/neditor/net/config.json",
                    isCacheConfig: false,
                    basePath: WebHostEnvironment.WebRootPath + "/admin/libs/neditor/net/"
                );
            #endregion

            #region 取消默认验证Api 接收参数模型 的 验证特性 如有 [ApiController]
            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
            #endregion

            //配置文件大小限制
            //services.Configure<FormOptions>(options =>
            //{
            //    options.ValueLengthLimit = int.MaxValue;
            //    options.MultipartBodyLengthLimit = int.MaxValue;// 60000000; 
            //    options.MultipartHeadersLengthLimit = int.MaxValue;
            //});

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime iHostApplicationLifetime)
        {



            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            #region 使用中间件
            app.UseMiddleware<HZYAppMiddleware>();
            #endregion

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllerRoute(
                //    name: "areas",
                //    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Authorization}/{action=Index}/{id?}");

                #region SignalR

                endpoints.MapHub<ChatHub>("/chatHub");

                #endregion

            });

            #region App 生命周期
            //程序启动
            iHostApplicationLifetime.ApplicationStarted.Register(() =>
            {
                Tools.Log.Write("App启动");
            });
            //程序正在结束中
            iHostApplicationLifetime.ApplicationStopping.Register(() =>
            {
                Tools.Log.Write("App结束中...");
            });
            //程序已结束
            iHostApplicationLifetime.ApplicationStopped.Register(() =>
            {
                Tools.Log.Write("App已结束");
            });
            //iHostApplicationLifetime.StopApplication();//停止程序
            #endregion

            #region Swagger
            ////启用中间件服务生成Swagger作为JSON终结点
            //app.UseSwagger();
            ////启用中间件服务对swagger-ui，指定Swagger JSON终结点
            //app.UseSwaggerUI(option =>
            //{
            //    foreach (var item in _VersionList) option.SwaggerEndpoint($"{item}/swagger.json", item);
            //    option.RoutePrefix = "swagger";
            //});
            #endregion

            #region 使用跨域 警告: 通过终结点路由，CORS 中间件必须配置为在对UseRouting和UseEndpoints的调用之间执行。 配置不正确将导致中间件停止正常运行。
            //app.UseCors("HZYCors");
            #endregion


        }
    }
}
