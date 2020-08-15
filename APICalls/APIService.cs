using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;

namespace APICalls
{
    internal class APIService : ServiceBase
    {
        public APIService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public APIService()
        {

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }


        protected override void OnStart(string[] args)
        {

        }

        protected override void OnStop()
        {

        }
    }
}
