/*
 * *******************************************************
 *
 * ���ߣ�hzy
 *
 * ��Դ��ַ��https://gitee.com/hzy6
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
            //���� AppConfigs
            AppConfig.AdminConfig = Configuration.GetSection("AppConfig").Get<AdminConfig>();
            AppConfig.LDAP = Configuration.GetSection("LDAP").Get<LDAP>();
            //AppConfig.WebPath = Configuration.GetSection("WebPath").Get<WebUrl>();
            #endregion

            //����mysql
            services.AddDapper("MSSQL1", m =>
            {
                m.ConnectionString = Configuration.GetSection("DapperConnectionStrings").Get<MSSQL>().MSSQL1;// ["DapperConnectionStrings:MSSQL1"];
                m.DbType = DbStoreType.SqlServer;
            });



            #region ע�� EFCore

            services.AddDbContext<EFCoreContext>(options =>
            {
                options
                .UseSqlServer(Configuration.GetConnectionString(nameof(EFCoreContext)))
                .UseLoggerFactory(efLogger)
                ;
                //�޸���  , b => b.MigrationsAssembly("HZY.Models")
                // options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            //services.AddIdentity<ApplicationUser, IdentityRole>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>()
            //    .AddDefaultTokenProviders();

            services.AddScoped(typeof(DefaultRepository<>));
            // services.AddScoped(typeof(IRepository<,>), typeof(DefaultRepository<>));
            #endregion

            #region ע�� ҵ�� ����
            services.ServiceStart(typeof(Startup));
            #endregion

            #region �������� ���ÿ�����
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
                        ValidateIssuer = true,//�Ƿ���֤Issuer
                        ValidateAudience = true,//�Ƿ���֤Audience
                        ValidateLifetime = true,//�Ƿ���֤ʧЧʱ��
                        ValidateIssuerSigningKey = true,//�Ƿ���֤SecurityKey
                        ValidAudience = AppConfig.AdminConfig.JwtKeyName,//Audience
                        ValidIssuer = AppConfig.AdminConfig.JwtKeyName,//Issuer���������ǰ��ǩ��jwt������һ��
                        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(AppConfig.AdminConfig.JwtSecurityKey))//�õ�SecurityKey
                    };

                    #region SignalR ���� jwt
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

            #region Swagger ע��Swagger������������һ���Ͷ��Swagger �ĵ�
            services.AddSwaggerGen(options =>
            {
                foreach (var item in _VersionList)
                {
                    options.SwaggerDoc(item, new OpenApiInfo { Title = "HZY.Admin" });
                }
                //Ϊ Swagger JSON and UI����xml�ĵ�ע��·��
                var xmlPath = Path.Combine(System.AppContext.BaseDirectory, "HZY.Admin.xml");
                var xmlPath1 = Path.Combine(System.AppContext.BaseDirectory, "HZY.Models.xml");
                options.IncludeXmlComments(xmlPath, true);
                options.IncludeXmlComments(xmlPath1, true);

                #region Jwt token ����
                //option.OperationFilter<AppService.SwaggerParameterFilter>(); // ��ÿ���ӿ�������Ȩ�봫������ı���
                //
                options.OperationFilter<AddResponseHeadersFilter>();
                options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                //����Ҫ���������ð�ȫУ�飬��֮ǰ�İ汾��һ��
                options.OperationFilter<SecurityRequirementsOperationFilter>();
                //���� oauth2 ��ȫ����
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT��Ȩ(���ݽ�������ͷ�н��д���) ֱ�����¿�������Bearer {token}��ע������֮����һ���ո�\"",
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
                //����ѭ������
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                //��ʹ���շ���ʽ��key
                options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                //����ʱ���ʽ
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });

            #region �м�� ע��
            services.AddTransient<HZYAppMiddleware>();
            #endregion

            #region �Զ�����ͼ
            //�Զ��� ��ͼ 
            services.Configure<Microsoft.AspNetCore.Mvc.Razor.RazorViewEngineOptions>(item =>
            {
                //ϵͳ����
                item.ViewLocationFormats.Add("/Views/Sys/{1}/{0}.cshtml");
                //��������
                item.ViewLocationFormats.Add("/Views/Base/{1}/{0}.cshtml");

                item.ViewLocationFormats.Add("/Views/Activity/{1}/{0}.cshtml");
            });
            #endregion

            #region SignalR
            services.AddSignalR();
            #endregion

            #region Ueditor
            //Ueditor  �༭�� ����� ע��  configFileRelativePath: "wwwroot/admin/libs/nUeditor/net/config.json", isCacheConfig: false, basePath: "C:/basepath"
            services.AddUEditorService(
                    configFileRelativePath: WebHostEnvironment.WebRootPath + "/admin/libs/neditor/net/config.json",
                    isCacheConfig: false,
                    basePath: WebHostEnvironment.WebRootPath + "/admin/libs/neditor/net/"
                );
            #endregion

            #region ȡ��Ĭ����֤Api ���ղ���ģ�� �� ��֤���� ���� [ApiController]
            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
            #endregion

            //�����ļ���С����
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

            #region ʹ���м��
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

            #region App ��������
            //��������
            iHostApplicationLifetime.ApplicationStarted.Register(() =>
            {
                Tools.Log.Write("App����");
            });
            //�������ڽ�����
            iHostApplicationLifetime.ApplicationStopping.Register(() =>
            {
                Tools.Log.Write("App������...");
            });
            //�����ѽ���
            iHostApplicationLifetime.ApplicationStopped.Register(() =>
            {
                Tools.Log.Write("App�ѽ���");
            });
            //iHostApplicationLifetime.StopApplication();//ֹͣ����
            #endregion

            #region Swagger
            ////�����м����������Swagger��ΪJSON�ս��
            //app.UseSwagger();
            ////�����м�������swagger-ui��ָ��Swagger JSON�ս��
            //app.UseSwaggerUI(option =>
            //{
            //    foreach (var item in _VersionList) option.SwaggerEndpoint($"{item}/swagger.json", item);
            //    option.RoutePrefix = "swagger";
            //});
            #endregion

            #region ʹ�ÿ��� ����: ͨ���ս��·�ɣ�CORS �м����������Ϊ�ڶ�UseRouting��UseEndpoints�ĵ���֮��ִ�С� ���ò���ȷ�������м��ֹͣ�������С�
            //app.UseCors("HZYCors");
            #endregion


        }
    }
}
