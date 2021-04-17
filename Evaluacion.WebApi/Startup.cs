using Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.Configuration;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Personas.Configuration;
using Evaluacion.Infraestructura.Datos.Persistencia.Core.Base.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Evaluacion.WebApi
{
    public class Startup
    {
        private readonly string MiCors = "MiCords";
        public IConfiguration Configuration { get; }
        public IHostEnvironment Environment { get; }
        public Startup(IHostEnvironment env)
        {
            Environment = env;
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings{env.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();
            //.EnableSubstitutions();
            Configuration = configurationBuilder.Build();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Evaluacion.WebApi", Version = "v1" })
            );
            services.AddRazorPages();
            services.AddServerSideBlazor();

            var dbSettings = Configuration.GetSection("DbSettings").Get<DbSettings>();
            //var ClientSettings = Configuration.GetSection("ClientSettings").Get<HttpClientSettings>();

            services.ConfigurePersonasService(dbSettings);
            services.ConfigureGenericasService(dbSettings);
            //services.ConfigureHttpClientService(ClientSettings);

            services.AddCors(options =>
                options.AddPolicy(name: MiCors, builder => builder.WithOrigins("*")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Evaluacion.WebApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
                endpoints.MapControllers()
            );
            app.UseCors(MiCors);
        }
    }
}
