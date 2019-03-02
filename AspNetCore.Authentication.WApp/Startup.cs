using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Authentication.WApp.Services;
using AspNetCore.Authentication.WApp.Services.Clients;
using AspNetCore.KeycloakAuthentication;
using AspNetCore.KeycloakAuthentication.Configuration;
using AspNetCore.KeycloakAuthentication.Handlers;
using AspNetCore.KeycloakAuthentication.Policies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNetCore.Authentication.WApp
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
            var clientId = "microservice-one";

            services.AddKeycloakAuthentication(new KeycloakOptions
            {
                Realm = "prueba",
                ClientId = clientId,
                ClientSecret = "388755d5-fc3d-450d-8a0d-c0adb8c72c80",
                KeycloakUrl = "http://artemis:8085/auth/",
                EnableGetTokenAuto = true,
                EnableRefreshToken = true
            });

            services.AddHttpClient();
            var uri2 = Configuration["UrlServices:microservices-two"];
            var uri3 = Configuration["UrlServices:microservices-three"];

            services.AddTransient<ITestService, TestService>();

            services.AddHttpClient<IMicroserviceTwo>(c => c.BaseAddress = new Uri(uri2)) //Inyectamos un HtttpClient con la URI base 
                .AddTypedClient(c => Refit.RestService.For<IMicroserviceTwo>(c)) //Convierte en instancia la interface
                .AddHttpKeycloakGetAutoTokenHandler(); //Agregamos el handler que se encargará de obtener el token o enviar el token

            services.AddHttpClient<IMicroserviceThree>(c => c.BaseAddress = new Uri(uri3)) //Inyectamos un HtttpClient con la URI base 
                .AddTypedClient(c => Refit.RestService.For<IMicroserviceThree>(c)) //Convierte en instancia la interface
                .AddHttpKeycloakGetAutoTokenHandler(); //Agregamos el handler que se encargará de obtener el token o enviar el token

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

            app.UseKeycloak(); //Habilitar la autenticación de Keycloak
            //app.UseHttpsRedirection();
            app.UseMvc();
        }        
    }
}
