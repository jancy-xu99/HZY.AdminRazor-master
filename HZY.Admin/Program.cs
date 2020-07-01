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
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HZY.Admin
{
    using HZY.EFCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using NLog.Web;
    using Toolkit;

    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            var host = CreateHostBuilder(args).Build();

            CreateDbIfNotExists(host);

            try
            {
                //����NLog
                NLogHelper.Set(logger);
                logger.Debug("��ʼ�� Main !");
                host.Run();
            }
            catch (Exception exception)
            {
                if (!(exception is MessageBox))
                {
                    //NLog: catch setup errors
                    logger.Error(exception, "�����쳣��ֹͣ����!");
                }
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .ConfigureWebHostDefaults(webBuilder =>
               {

                   webBuilder.UseStartup<Startup>()
                   .ConfigureLogging(logging =>
                   {
                       logging.ClearProviders();
                       logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                   })
                   .UseNLog();  // NLog: Setup NLog for Dependency injection;;
                   //.UseUrls("http://*:8888");
               });

        /// <summary>
        /// Ef ����
        /// </summary>
        /// <param name="host"></param>
        private static void CreateDbIfNotExists(IHost host)
        {
            // using (var scope = host.Services.CreateScope())
            // {
            //     var services = scope.ServiceProvider;

            //     try
            //     {
            //         var context = services.GetRequiredService<EFCoreContext>();
            //         context.Database.EnsureCreated();
            //     }
            //     catch (Exception ex)
            //     {
            //         var logger = services.GetRequiredService<ILogger<Program>>();
            //         logger.LogError(ex, "�������ݿ�ʱ����.");
            //     }
            // }
        }






    }
}
