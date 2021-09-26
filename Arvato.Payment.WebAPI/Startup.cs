using Arvato.Payment.Application.Services;
using Arvato.Payment.Application.Services.Interfaces;
using Arvato.Payment.WebAPI.Presenters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arvato.Payment.WebAPI
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
            services.AddCors();

            services.AddControllers().ConfigureApiBehaviorOptions(options =>{
                options.InvalidModelStateResponseFactory = actioncontext =>
                {
                    return CustomErrorResponse(actioncontext);
                };
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Arvato.Payment.WebAPI", Version = "v1" });
            });
            #region
            services.AddScoped<IPaymentService, PaymentService>();

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Arvato.Payment.WebAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) 
                .AllowCredentials()); 
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private BadRequestObjectResult CustomErrorResponse(ActionContext actionContext)
        {
            OutPutContentResult<string> output = new();

            var errors = actionContext.ModelState
             .Where(modelError => modelError.Value.Errors.Count > 0)
             .Select(modelError => new ErrorContent
             {
                 ErrorField = modelError.Key,
                 ErrorDescription = modelError.Value.Errors.FirstOrDefault().ErrorMessage
             }).ToList();
            output.errors = errors;
            return new BadRequestObjectResult(output);
        }
    }
}
