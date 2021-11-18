using DailyLoan.Model.Entities.DailyLoan;
using DailyLoan.Repository;
using DailyLoan.Repository.Interfaces;
using DailyLoan.Repository.Interfaces;
using DailyLoan.Service;
using DailyLoan.Service.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Serilog.Events;


namespace DailyLoan
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddDbContext<DailyLoanContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DailyLoanConnection")));

            #region DependencyInjectionConfig

            // DependencyInjection Service
            services.AddScoped<IManagementService, ManagementService>();
            services.AddScoped<ILogInService, LogInService>();
            services.AddScoped<IPayService, PayService>();
            services.AddScoped<IOperationService, OperationService>();

            // DependencyInjection Repository
            services.AddScoped<IManagementRepo, ManagementRepo>();
            services.AddScoped<ILogInRepo, LogInRepo>();
            services.AddScoped<IPayRepo, PayRepo>();
            services.AddScoped<IOperationRepo, OperationRepo>();

            #endregion

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(7200);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddControllersWithViews();

            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors("DevelopmentCors");
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
