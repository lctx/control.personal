using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using control.personal.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System.Text;
using control.personal.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using control.personal.Services;
using Microsoft.AspNetCore.Authorization;

namespace control.personal
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var key = Encoding.ASCII.GetBytes(Configuration.GetValue<string>("SecretKey"));
            if (_env.IsDevelopment())
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlite(
                        Configuration.GetConnectionString("DefaultConnection")));
            }
            else
            {
                var serverVersion = new MariaDbServerVersion(new Version(10, 3, 30));
                services.AddDbContext<ApplicationDbContext>(
                    dbContextOptions => dbContextOptions
                    .UseMySql(Configuration.GetConnectionString("ProductionConnection"), serverVersion)
                    .EnableSensitiveDataLogging() // <-- These two calls are optional but help
                    .EnableDetailedErrors()       // <-- with debugging (remove for production).
                    );
            }
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddDefaultIdentity<Usuario>
                (options =>
                    {
                        //configuracion para desabilitar el uso de una contraseña segura para asignar una contraseña por defecto 
                        options.SignIn.RequireConfirmedAccount = true;
                        options.Password.RequireDigit = false;
                        options.Password.RequiredLength = 4;
                        options.Password.RequiredUniqueChars = 0;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireUppercase = false;
                    })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddAuthentication(options => options.RequireAuthenticatedSignIn = true)
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            services.AddAuthorization(options =>
            {
                //habilitación para requerir un usuario logeado para acceder a la app
                /*options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();*/
                options.AddPolicy("API", policy =>
                {
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("Sensor");
                });
                options.AddPolicy("Politica-Administrador", policy =>
                {
                    policy.RequireRole("Administrador");
                });
                options.AddPolicy("Politica-Empleado", policy =>
                {
                    policy.RequireRole("Empleado");
                });
                options.AddPolicy("Politica-AdministracionIdentificaciones", policy =>
                {
                    policy.RequireUserName(Configuration["Administracion:Identificaciones"]);
                });
            });
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            });
            services.AddRazorPages();
            services.AddHttpClient<TelegramService>
                (x =>
                //si uso http salta un  bad request
                x.BaseAddress = new Uri("https://api.telegram.org/bot" + Configuration["Telegram:Botid"]));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}
