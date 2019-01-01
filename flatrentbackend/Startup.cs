﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using FlatRent.Interfaces;
using FlatRent.Repositories;
using FlatRent.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;

namespace FlatRent
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
            services.AddAutoMapper();
            services.AddSingleton(Log.Logger);
            services.AddDbContext<DataContext>(opts => opts.UseNpgsql(Configuration.GetConnectionString("DataContext")).UseLazyLoadingProxies());

            services.AddAuthorization(options =>
            {
                options.AddPolicy("CustomerService", policyOptions =>
                    policyOptions.AddRequirements(
                        new RolesAuthorizationRequirement(new[] {"Administrator", "CustomerService"}))
                );
                options.AddPolicy("Accounting", policyOptions =>
                    policyOptions.AddRequirements(
                        new RolesAuthorizationRequirement(new[] {"Administrator", "Accounting"}))
                );
                options.AddPolicy("Supply", policyOptions =>
                    policyOptions.AddRequirements(
                        new RolesAuthorizationRequirement(new[] {"Administrator", "Supply"}))
                );
                options.AddPolicy("Sales", policyOptions =>
                    policyOptions.AddRequirements(
                        new RolesAuthorizationRequirement(new[] {"Administrator", "Sales"}))
                );
                options.AddPolicy("Client", policyOptions =>
                    policyOptions.AddRequirements(
                        new RolesAuthorizationRequirement(new[] {"Administrator", "Client"}))
                );
                options.AddPolicy("Employee", policyOptions =>
                    policyOptions.AddRequirements(
                        new RolesAuthorizationRequirement(new[] {"Administrator", "Employee"}))
                );
            });

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["ServiceConfig:Issuer"],
                    ValidAudience = Configuration["ServiceConfig:Audience"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["ServiceConfig:JwtSecret"])),
                };
            });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IFlatRepository, FlatRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IUserService, UserService>();            


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "FlatRent API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };
                c.AddSecurityRequirement(security);
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });

            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options => {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });;
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

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var data = scope.ServiceProvider.GetService<DataContext>();
                data.Database.Migrate();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "FlatRent API V1");
            });

            app.UseCors(x => x
                .WithOrigins("http://localhost:5000", "https://localhost:5001", "http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials());

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
