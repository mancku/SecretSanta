using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SecretSanta;
using SecretSanta.Communications;
using SecretSanta.Communications.Email;
using SecretSanta.Communications.SMS;

namespace SecretSantaWebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddScoped<ISecretSantaGenerator, SecretSantaGenerator>();
            services.AddScoped<ISecretSantaService, SecretSantaService>();

            services.AddScoped<ICommunicationsService, CommunicationsService>();
            services.AddScoped<NexmoService>(x => new NexmoService(this.Configuration));
            services.AddScoped<SendGridService>(x => new SendGridService(this.Configuration));

            services.AddScoped<SenderServiceResolver>(serviceProvider => key =>
            {
                switch (key)
                {
                    case SenderType.Email:
                        return serviceProvider.GetService<SendGridService>();
                    case SenderType.SMS:
                        return serviceProvider.GetService<NexmoService>();
                    default:
                        throw new ArgumentOutOfRangeException(nameof(key), key, null);
                }
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}