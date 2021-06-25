using AutomatedDeployment.Api.Services;
using AutomatedDeployment.Core;
using AutomatedDeployment.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace AutomatedDeployment.Api
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
            services.AddInfrastructure(Configuration);
            services.AddServices();     
            services.AddScoped<IReplaceServices, ReplaceServices>();
            services.AddScoped<IRollbackService, RollBackService>();
            services.AddScoped<IBackupServices, BackupServices>();

            //services.AddScoped<IDeletefilesrepository, Deletefilesrepository>();

            services.AddCors(options =>
            {
                options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });
            services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AutomatedDeployment.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json","AutomatedDeployment.Api v1"));

            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();
            app.UseCors("Open");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
