using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenIddict.Validation.AspNetCore;
using Sofisoft.Erp.MeasureUnits.Api.Infrastructure.AutofacModules;
using Sofisoft.Logging;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Sofisoft.Erp.MeasureUnits.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            var cultureInfo = new CultureInfo("es-co");

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            if(_env.IsDevelopment())
            {
                services.AddDataProtection()
                    .UseEphemeralDataProtectionProvider();
            }
            else
            {
                services.AddDataProtection()
                    .PersistKeysToAzureBlobStorage(new Uri(Configuration["BlobSasUri"]));
            }

            services.AddSofisoft()
                .AddIdentity(options => {})
                .AddLogging(options =>
                {
                    options.SetBaseAddress(Configuration["Services:LoggingUrl"]);
                    options.SetTokenValue("dXNybG9nZ2luZzpBMTIzNDU2YQ==");
                    options.SetSource(Assembly.GetExecutingAssembly().GetName().Name);
                })
                .AddMongoDb(options =>
                {
                    options.SetConnectionString(Configuration["ConnectionString"]);
                    options.SetDatabase(Configuration["Database"]);
                });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.WithOrigins(Configuration["AllowedOrigins"].Split(";"))
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            });

            services.AddOpenIddict()
            .AddValidation(options => {
                options.SetIssuer(Configuration["Services:IdentityUrl"]);
                options.AddAudiences("sofisoft");

                options.AddEncryptionKey(new SymmetricSecurityKey(
                    Convert.FromBase64String(Configuration["EncryptionKey"])));

                options.UseSystemNetHttp();
                options.UseAspNetCore();
            });

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                });
            
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo 
                {
                    Title = "Measure Units API",
                    Version = "v1",
                    Description = "Specifying services for measure units.",
                    Contact = new OpenApiContact {
                        Email = "jabanto@sofisofttech.com", 
                        Name = "Sofisoft Technologies SAC", 
                        Url = new Uri(Configuration["HomePage"])
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new MediatorModule());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger()
                .UseSwaggerUI(c =>
                    {
                        c.DocumentTitle = "Sofisoft - MeasureUnitsAPI";
                        c.RoutePrefix = string.Empty;
                        c.SupportedSubmitMethods(Array.Empty<SubmitMethod>());
                        c.SwaggerEndpoint(Configuration["SwaggerEndPoint"], "Measure Units V1");
                        c.DefaultModelsExpandDepth(-1);
                    });

            app.UseMiddleware<SofisoftLoggingMiddleware>();
            
            app.UseCors("CorsPolicy");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
