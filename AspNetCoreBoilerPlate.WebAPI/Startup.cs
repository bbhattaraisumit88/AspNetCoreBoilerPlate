using AspNetCoreBoilerPlate.Domain.Models;
using AspNetCoreBoilerPlate.EFCore;
using AspNetCoreBoilerPlate.Infrastructure.Repositories.Implementation;
using AspNetCoreBoilerPlate.Infrastructure.Repositories.Interface;
using AspNetCoreBoilerPlate.Service.Implementation;
using AspNetCoreBoilerPlate.Service.Interface;
using AspNetCoreBoilerPlate.WebAPI.IdentityServerConfiguration;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;

namespace AspNetCoreBoilerPlate.WebAPI
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
            services.AddDbContext<ApplicationDbContext>(options =>
                           options.UseSqlServer(Configuration.GetConnectionString("TestConnection")));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddCors();
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "Now CFO API", Version = "v1" });
                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(security);
            });
            services.AddIdentity<AppUser, AppRole>(o =>
            {
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 5;
            }).AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                  .AddInMemoryPersistedGrants()
                  .AddInMemoryIdentityResources(Config.GetIdentityResources())
                  .AddInMemoryApiResources(Config.GetApiResources())
                  .AddInMemoryClients(Config.GetClients())
                  .AddAspNetIdentity<AppUser>();

            services.AddTransient<IProfileService, IdentityClaimsProfileService>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddIdentityServerAuthentication(o =>
             {
                 o.Authority = "http://localhost:50570"; //crucial part
                 o.ApiName = "api1";
                 o.RequireHttpsMetadata = false;
             });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
                options.AddPolicy("Guest", policy => policy.RequireRole("Admin", "Guest"));
            });
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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseCors(
               option => option.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
            );
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Now CFO API V1");
            });
            app.UseMvc();
        }
    }
}