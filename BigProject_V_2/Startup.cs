using BigProject_V_2.DataAccessLayer.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using BigProject_V_2.DataAccessLayer.UnitOfWork;
using BigProject_V_2.BusinessLayer.Managers;
using BigProject_V_2.BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using BigProject_V_2.DataAccessLayer.Dtos;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace BigProject_V_2
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0).AddJsonOptions(options =>
            {
                var resolver = new JsonSerializerSettings().ContractResolver;
                if (resolver != null)
                    (resolver as DefaultContractResolver).NamingStrategy = null;
            });

            services.AddControllers().AddNewtonsoftJson();
            services.Configure<IISOptions>(options =>
            {
                options.AutomaticAuthentication = false;
            });
            services.AddControllersWithViews();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";///ClientApp";
            });//"Data Source=SQL6007.site4now.net;Initial Catalog=DB_A4F3F8_ryabishcuk2;User Id=DB_A4F3F8_ryabishcuk2_admin;Password=kracken12;"

            services.AddCors(options => 
            options.AddPolicy("EnableCORS", builder =>
            {
                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().Build(); // in tutorial builder should aplly AllowCredentials also 
            }));

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("BigProject_V_2.DataAccessLayer")));
           
            services.AddIdentity<IdentityUser, IdentityRole>(
                options => {
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 5;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.User.RequireUniqueEmail = true;
                    
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(2);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.AllowedForNewUsers = true;
                }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
                       
            services.AddScoped<IUnitOfWork, UnitOfWork>();    
           
            services.AddScoped<IProductManager, ProductManager>();

            services.AddScoped<IEmailManager, EmailManager>();

            var appsettingsSection = Configuration.GetSection("ApplicationSettings");

            services.Configure<ApplicationSettings>(appsettingsSection);

            var appSettings = appsettingsSection.Get<ApplicationSettings>();

            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            // Authentication middleware
            services.AddAuthentication(options => 
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => 
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = appSettings.Site,
                    ValidAudience = appSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });
            
            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireLoggedIn", policy => policy.RequireRole("Admin", "Customer", "Moderator").RequireAuthenticatedUser());

                options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Admin").RequireAuthenticatedUser());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            

            app.UseCors("EnableCORS");
           
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseAuthorization();

            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
