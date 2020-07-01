using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace HZY.Toolkit.ToolKits
{
    public class ConfigurationHelper
    {
        public static IConfiguration config { get; set; }

        //static ConfigurationHelper()
        //{
        //    //IHostingEnvironment env = MyServiceProvider.ServiceProvider.GetRequiredService<IHostingEnvironment>();
        //    config = new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        //        .AddEnvironmentVariables()
        //        .Build();
        //}

        //public static T GetAppSettings<T>(string key) where T : class, new()
        //{
        //    var appconfig = new ServiceCollection().AddOptions().Configure<T>(config.GetSection(key)).BuildServiceProvider()
        //        .GetService<IOptions<T>>().Value;
        //    return appconfig;



        //}




    }


    public class MyServiceProvider
    {
        public static IServiceProvider ServiceProvider { get; set; }
    }

}
