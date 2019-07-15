using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCar.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RentCar.Models;

namespace RentCar
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(50);
                options.LoginPath = "/Usuarios/Login";
                options.SlidingExpiration = true;
            });

            services.Configure<IdentityOptions>(options =>
            {
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";      // quais caracteres permitidos
                options.User.RequireUniqueEmail = true;                                     // email único, false por default	
                options.Password.RequireDigit = false;                                       // números obrigatórios na senha, true por default
                options.Password.RequiredLength = 6;                                        // tamanho mínimo da senha, 6 por default
                options.Password.RequireLowercase = false;                                   // caracteres minúsculos obrigatórios na senha, true por default		
                options.Password.RequireUppercase = false;                                   // caracteres maiúsculos obrigatórios na senha, true por default
                options.Password.RequireNonAlphanumeric = false;                             // caracteres especiais obrigatórios na senha, true por default
                options.Password.RequiredUniqueChars = 1;                                   // qtd de caracteres repetidos na senha, 1 por default
            });

            services.AddDbContext<Context>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<User, Role>().AddDefaultUI().AddEntityFrameworkStores<Context>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddScoped<SignInManager<User>, SignInManager<User>>();
            services.AddScoped<UserManager<User>, UserManager<User>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
