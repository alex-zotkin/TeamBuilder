using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TeamBuilder.Middlewares;
using TeamBuilder.Models;

namespace TeamBuilder
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
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DataBaseContext>(options => 
                                                   options.UseSqlServer(connection));
            services.AddControllersWithViews();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePagesWithRedirects("error/{0}");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<IsUserLogin>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "error",
                    pattern: "/error/{code}",
                    defaults: new { controller = "Error", action = "Index" });

                endpoints.MapControllerRoute(
                    name: "login",
                    pattern: "/login",
                    defaults: new {controller="Login", action="Index"});

                endpoints.MapControllerRoute(
                    name: "logout",
                    pattern: "/logout",
                    defaults: new { controller = "Login", action = "Logout" });

                endpoints.MapControllerRoute(
                    name: "auth",
                    pattern: "/auth",
                    defaults: new { controller = "Auth", action = "Index" });


                endpoints.MapControllerRoute(
                    name: "developers",
                    pattern: "/developers",
                    defaults: new { controller = "Developers", action = "Index" });

                endpoints.MapControllerRoute(
                    name: "home",
                    pattern: "/",
                    defaults: new { controller = "Home", action = "Index"});

                endpoints.MapControllerRoute(
                   name: "profileEdit",
                   pattern: "/profile",
                   defaults: new { controller = "Profile", action = "Edit" });

                endpoints.MapControllerRoute(
                   name: "addProject",
                   pattern: "/addProject",
                   defaults: new { controller = "Project", action = "Add" });

                endpoints.MapControllerRoute(
                   name: "project",
                   pattern: "/project/{id}",
                   defaults: new { controller = "Project", action = "Index" });

                endpoints.MapControllerRoute(
                  name: "allInfoAboutProject",
                  pattern: "/allInfoAboutProject/{id}",
                  defaults: new { controller = "Project", action = "AllInfoAboutProject" });

                endpoints.MapControllerRoute(
                 name: "changeProjectName",
                 pattern: "/changeProjectName/{id}/{newName}",
                 defaults: new { controller = "Project", action = "ChangeProjectName" });

                endpoints.MapControllerRoute(
                 name: "addInProjectAdmin",
                 pattern: "/addInProjectAdmin/{id}/{VkId}",
                 defaults: new { controller = "Project", action = "AddInProjectAdmin" });

                endpoints.MapControllerRoute(
                 name: "deleteFromProjectAdmin",
                 pattern: "/deleteFromProjectAdmin/{id}/{VkId}",
                 defaults: new { controller = "Project", action = "DeleteFromProjectAdmin" });

                endpoints.MapControllerRoute(
                 name: "addInProjectNews",
                 pattern: "/addInProjectNews/{id}/{VkId}/{text}",
                 defaults: new { controller = "Project", action = "AddInProjectNews" });

                endpoints.MapControllerRoute(
                 name: "deleteInProjectNews",
                 pattern: "/deleteInProjectNews/{newId}",
                 defaults: new { controller = "Project", action = "DeleteInProjectNews" });
            });
        }
    }
}
