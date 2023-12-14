using System;
using HealthyLife.Context;
using HealthyLife.Manager.Admin;
using HealthyLife.Manager.App;
using HealthyLife.Service.Admin;
using HealthyLife.Service.App;
using HealthyLife.Service.Backup;
using HealthyLife.Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;

namespace HealthyLife
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "HealthyLife",
                    Version = "1.0.0"
                });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
 
            services.AddControllers();
            services.AddTransient<IAppManager,AppManager>();
            services.AddTransient<IAppService, AppService>();
            services.AddTransient<IAdminManager, AdminManager>();
            services.AddTransient<IBackupService, BackupService>();
            services.AddTransient<IAdminService, AdminService>();
            services.AddTransient<IJsonEncryptionService, EncryptionService>();
            services.AddTransient<IEncryptionService, EncryptionService>();
            services.AddHttpContextAccessor();
            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            IConfigurationRoot configuration = builder.Build();
            var dbContextOptions = new DbContextOptionsBuilder().UseSqlServer(configuration["ConnectionString"]).Options;
         
            services.AddScoped<DevContext>(x=>new DevContext(dbContextOptions, configuration));
            services.AddScoped<DapperContext>(x=>new DapperContext(configuration["ConnectionString"]));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = configuration["Issuer"],
                        ValidateAudience = true,
                        ValidAudience = configuration["Audience"],
                        ValidateLifetime = true,
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(configuration["Key"]),
                        ValidateIssuerSigningKey = true
                    };
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.DocumentTitle = "HealthyLife";
                options.RoutePrefix = "";
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger UI Demo");
                options.DocExpansion(DocExpansion.List);
            });
            app.UseRouting();
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseHttpsRedirection(); 
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}