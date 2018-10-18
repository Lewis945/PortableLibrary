using Askmethat.Aspnet.JsonLocalizer.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PortableLibrary.Core.Database;
using PortableLibrary.Core.Infrastructure.Membership;
using PortableLibrary.Core.Infrastructure.SimpleServices;
using PortableLibrary.Core.SimpleServices;
using System;
using System.Globalization;
using System.Text;

namespace PortableLibrary.Apps.WebApi
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
            services.AddJsonLocalization(options =>
            {
                options.CacheDuration = TimeSpan.FromMinutes(15);
                options.ResourcesPath = "Locales";
                options.FileEncoding = Encoding.UTF8;
            });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("ru")
                };

                options.DefaultRequestCulture = new RequestCulture("en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            services.AddCors();
            services.AddAutoMapper();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContext<PortableLibraryDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default")));

            services.AddScoped<ILibraryService>(provider => new LibraryService(provider.GetService<PortableLibraryDataContext>()));
            services.AddScoped<IBookService>(provider => new BookService(provider.GetService<PortableLibraryDataContext>()));
            services.AddScoped<ITvShowService>(provider => new TvShowService(provider.GetService<PortableLibraryDataContext>()));

            MembershipInitializer.Register(services, Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseRequestLocalization();

            app.UseCors(x => x
             .AllowAnyOrigin()
             .AllowAnyMethod()
             .AllowAnyHeader()
             .AllowCredentials());

            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
