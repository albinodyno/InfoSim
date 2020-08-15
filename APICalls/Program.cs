using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.ServiceProcess;

namespace APICalls
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var service = new APIService())
            {
                ServiceBase.Run(service);
            }
        }

        //static void Main(string[] args)
        //{
        //    using (var service = new APIService())
        //    {
        //        if (args.Length > 0)
        //            if (args[0] == "console")
        //            {
        //                BuildWebHost(args).Run();
        //            }
        //    }
        //}

        //public static IWebHost BuildWebHost(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args)
        //        .UseStartup<RRService>()
        //        .UseUrls("http://localhost:9090")
        //        .Build();
    }
}
