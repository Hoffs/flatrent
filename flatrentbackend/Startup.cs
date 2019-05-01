using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using FlatRent.Entities;
using FlatRent.Extensions;
using FlatRent.Repositories;
using FlatRent.Repositories.Interfaces;
using FlatRent.Services;
using FlatRent.Services.Interfaces;
using FlatRent.Utilities;
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
using Newtonsoft.Json.Serialization;
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
//            services.AddDbContext<DataContext>(opts => opts.UseNpgsql(Configuration.GetConnectionString("DataContext")).UseLazyLoadingProxies());
//            services.AddDbContext<DataContext>(opts => opts.UseNpgsql(Configuration.GetConnectionString("DataContext")).UseLazyLoadingProxies());
            services.AddDbContext<DataContext>(opts => opts.UseSqlServer(Configuration.GetConnectionString("DataContext")).UseLazyLoadingProxies());

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Administrator", policyOptions =>
                    policyOptions.AddRequirements(
                        new RolesAuthorizationRequirement(new[] { UserType.Administrator.Role }))
                );
                options.AddPolicy("User", policyOptions =>
                    policyOptions.AddRequirements(
                        new RolesAuthorizationRequirement(new[] { UserType.Administrator.Role, UserType.User.Role }))
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
            services.AddScoped<IAgreementRepository, AgreementRepository>();
            services.AddScoped<IUserService, UserService>();            
            services.AddScoped<IAttachmentRepository, AttachmentRepository>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<IInvoiceService, InvoiceService>();


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
                c.EnableAnnotations();
            });

            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options => {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });;
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = ctx => new CustomModelErrorResponse();
//                options.SuppressModelStateInvalidFilter = true;
            });
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
            app.ConfigureExceptionHandler(Log.Logger);
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DisplayOperationId();
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

            app.UseExceptionHandler(CustomExceptionHandler.Configure);
        }
    }
}
