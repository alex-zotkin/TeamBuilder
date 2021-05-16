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
                    name: "howitworks",
                    pattern: "/howitworks",
                    defaults: new { controller = "Howitworks", action = "Index" });

                endpoints.MapControllerRoute(
                   name: "profileEdit",
                   pattern: "/profile",
                   defaults: new { controller = "Profile", action = "Edit" });

                endpoints.MapControllerRoute(
                   name: "addProject",
                   pattern: "/addProject",
                   defaults: new { controller = "Project", action = "Add" });


                endpoints.MapControllerRoute(
                   name: "addTeamsInProject",
                   pattern: "/addteamsinproject",
                   defaults: new { controller = "Project", action = "AddTeamsInProject" });

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
                 name: "addInProjectJury",
                 pattern: "/addInProjectJury/{id}/{VkId}",
                 defaults: new { controller = "Project", action = "AddInProjectJury" });

                endpoints.MapControllerRoute(
                 name: "deleteFromProjectJury",
                 pattern: "/deleteFromProjectJury/{id}/{VkId}",
                 defaults: new { controller = "Project", action = "DeleteFromProjectJury" });

                endpoints.MapControllerRoute(
                 name: "addInProjectNews",
                 pattern: "/addInProjectNews/{id}/{VkId}/{text}",
                 defaults: new { controller = "Project", action = "AddInProjectNews" });

                endpoints.MapControllerRoute(
                 name: "deleteInProjectNews",
                 pattern: "/deleteInProjectNews/{newId}",
                 defaults: new { controller = "Project", action = "DeleteInProjectNews" });


                endpoints.MapControllerRoute(
                 name: "usersNotInProject",
                 pattern: "/usersnotinproject",
                 defaults: new { controller = "Project", action = "UsersNotInProject" });


                endpoints.MapControllerRoute(
                 name: "addUserInTeamById",
                 pattern: "/adduserinteambyid",
                 defaults: new { controller = "Project", action = "AddUserInTeamById" });

                endpoints.MapControllerRoute(
                 name: "deleteProject",
                 pattern: "/deleteproject",
                 defaults: new { controller = "Project", action = "DeleteProject" });



                endpoints.MapControllerRoute(
                 name: "deleteTeam",
                 pattern: "/deleteteam",
                 defaults: new { controller = "Project", action = "DeleteTeam" });

                endpoints.MapControllerRoute(
                 name: "joinTeam",
                 pattern: "/joinTeam/{VkId}/{TeamId}",
                 defaults: new { controller = "Project", action = "JoinTeam" });

                endpoints.MapControllerRoute(
                 name: "exitTeam",
                 pattern: "/exitTeam/{VkId}/{TeamId}",
                 defaults: new { controller = "Project", action = "ExitTeam" });


                endpoints.MapControllerRoute(
                 name: "changeTeam",
                 pattern: "/changeteam",
                 defaults: new { controller = "Project", action = "ChangeTeam" });


                endpoints.MapControllerRoute(
                 name: "allNotifications",
                 pattern: "/notifications/all",
                 defaults: new { controller = "Notifications", action = "AllNotifications" });

                endpoints.MapControllerRoute(
                 name: "deleteApplication",
                 pattern: "/notifications/deleteApplication/{TeamId}/{UserId}",
                 defaults: new { controller = "Notifications", action = "DeleteApplication" });

                endpoints.MapControllerRoute(
                 name: "checkApplication",
                 pattern: "/notifications/checkApplication/{TeamId}/{UserId}/{Successed}",
                 defaults: new { controller = "Notifications", action = "CheckApplication" });


                endpoints.MapControllerRoute(
                 name: "team",
                 pattern: "/project/{ProjectId}/team/{TeamId}",
                 defaults: new { controller = "Team", action = "Index" });

                
                endpoints.MapControllerRoute(
                 name: "allInfoAboutTeam",
                 pattern: "/allInfoAboutTeam/{TeamId}",
                 defaults: new { controller = "Team", action = "AllInfoAboutTeam" });


                endpoints.MapControllerRoute(
                 name: "changeInfoTeam",
                 pattern: "/changeInfoTeam/{TeamId}/{inputName}/{data}",
                 defaults: new { controller = "Team", action = "ChangeInfoTeam" });

                endpoints.MapControllerRoute(
                 name: "setTeamLead",
                 pattern: "/setteamlead",
                 defaults: new { controller = "Team", action = "SetTeamLead" });


                endpoints.MapControllerRoute(
                 name: "changeTeamImg",
                 pattern: "/changeTeamImg",
                 defaults: new { controller = "Team", action = "ChangeTeamImg" });


                endpoints.MapControllerRoute(
                 name: "sendMes",
                 pattern: "/sendMes",
                 defaults: new { controller = "Team", action = "SendMes" });


                endpoints.MapControllerRoute(
                 name: "delMes",
                 pattern: "/delMes",
                 defaults: new { controller = "Team", action = "DelMes" });


                endpoints.MapControllerRoute(
                 name: "saveLinks",
                 pattern: "/saveLinks",
                 defaults: new { controller = "Team", action = "SaveLinks" });


                endpoints.MapControllerRoute(
                 name: "delFile",
                 pattern: "/delFile",
                 defaults: new { controller = "Team", action = "DelFile" });


                endpoints.MapControllerRoute(
                 name: "loadFiles",
                 pattern: "/loadFiles",
                 defaults: new { controller = "Team", action = "LoadFiles" });


                endpoints.MapControllerRoute(
                 name: "marks",
                 pattern: "/project/{ProjectId}/marks",
                 defaults: new { controller = "Marks", action = "Index" });


                endpoints.MapControllerRoute(
                 name: "allInfoAboutMarks",
                 pattern: "/allInfoAboutMarks/{ProjectId}",
                 defaults: new { controller = "Marks", action = "AllInfoAboutMarks" });


                endpoints.MapControllerRoute(
                 name: "setMark",
                 pattern: "/setMark",
                 defaults: new { controller = "Marks", action = "SetMark" });


                endpoints.MapControllerRoute(
                 name: "openMarks",
                 pattern: "/openMarks",
                 defaults: new { controller = "Marks", action = "OpenMarks" });



                endpoints.MapControllerRoute(
                 name: "download",
                 pattern: "/download",
                 defaults: new { controller = "Marks", action = "Download" });
            });
        }
    }
}
