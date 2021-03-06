using System;
using System.IO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mobicon.Auth;
using Mobicon.Services;

namespace Mobicon
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(
            IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options => options.UseMySql(Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING") ?? Configuration.GetConnectionString("MySQL")));
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Login";
                    options.AccessDeniedPath = "/Configs";
                });

            services.Configure<LdapConfig>(Configuration.GetSection("ldap"));
            services.AddSingleton<LdapAuthenticationService>();
            services.AddTransient<ImportManager>();
            services.AddSingleton<ExportManager>();
            services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Fastest);
            services.AddResponseCompression();

            var appSettings = new AppSettings();
            Configuration.GetSection("settings").Bind(appSettings);
            services.Add(new ServiceDescriptor(typeof(AppSettings), appSettings));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IHostingEnvironment env,
            DataContext dbContext)
        {
            dbContext.Database.Migrate();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseResponseCompression();
            app.UseAuthentication();
            app.UseStaticFiles();

            app.UseMvc();
        }
    }
}
