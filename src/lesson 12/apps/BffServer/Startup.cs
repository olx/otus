using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProxyKit;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http;
using System.Net;

namespace BffServer
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddProxy();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseRouting();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("super secret key"));
            var parameters =  new TokenValidationParameters()
            {
                ValidateLifetime = false, 
                ValidateAudience = false,
                ValidateIssuer = false, 
                IssuerSigningKey = key
            };
            var handler = new JwtSecurityTokenHandler();
            app.RunProxy(context => {
  
                try
                {
                    var jwt = context.Request.Headers["Authorization"][0].Split(' ')[1];
                    handler.ValidateToken(jwt, parameters, out var securityToken);
                    var token = (JwtSecurityToken)securityToken;
                    var userId = token.Claims.Where(w => w.Type == "UserId").Select(s => s.Value).FirstOrDefault();
                    var forward = context.ForwardTo("http://profileserver:5004/");
                    forward.UpstreamRequest.Headers.Add("X-USER-ID", userId);
                    return forward.Send();

                }
                catch
                {
                    return Task.FromResult(new HttpResponseMessage(HttpStatusCode.Unauthorized));
                }
                
            }); 
        }
    }
}
