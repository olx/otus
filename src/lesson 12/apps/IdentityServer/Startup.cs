using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System.Text;
using DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using JwtExtension;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;

namespace IdentityServer
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
            services.AddControllers();

            var connectionString = this.Configuration.GetValue("OTUS_DATABASE", "");
            var version = "5.7.27-mysql";

            services.AddDbContextPool<DataContext>(options => options
                .UseMySql(connectionString, x => x.ServerVersion(version)));

            services.TryAddSingleton<ISystemClock, SystemClock>();
            services.AddIdentityCore<AppUser>().AddEntityFrameworkStores<DataContext>().AddSignInManager<SignInManager<AppUser>>();

            services.AddMvc(option =>
            {
                // Отключаем маршрутизацию конечных точек на основе endpoint-based logic из EndpointMiddleware
                // и продолжаем использование маршрутизации на основе IRouter. 
                option.EnableEndpointRouting = false;
                var policy = new AuthorizationPolicyBuilder()
                                    .RequireAuthenticatedUser().RequireAuthenticatedUser().Build();
                option.Filters.Add(new AuthorizeFilter(policy));
            }).SetCompatibilityVersion(CompatibilityVersion.Latest);
           
            services.AddScoped<IJwtGenerator, JwtGenerator>();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("super secret key"));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(
                    opt =>
                    {
                        opt.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = key,
                            ValidateAudience = false,
                            ValidateIssuer = false,
                        };
                    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                var context = services.GetService<DataContext>();
                var userManager = services.GetService<UserManager<AppUser>>();
                while (true)
                {
                    try
                    {                       
                        context.Database.Migrate();
                        DataSeed.SeedDataAsync(context, userManager).Wait();
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Thread.Sleep(TimeSpan.FromSeconds(2));
                    }
                }
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
        }
    }
}
