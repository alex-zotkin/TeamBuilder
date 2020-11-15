using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamBuilder.Models;

namespace TeamBuilder.Middlewares
{
    public class IsUserLogin
    {
        private readonly RequestDelegate _next;
        public IsUserLogin(RequestDelegate del)
        {
            _next = del;
        }

        public async Task InvokeAsync(HttpContext context, DataBaseContext db)
        {
            var Coockies = context.Request.Cookies;
            if(!Coockies.ContainsKey("UserData") || !Coockies.ContainsKey("AccessToken"))
            {
                context.Response.Cookies.Delete("UserData");
                context.Response.Cookies.Delete("AccessToken");
                if (context.Request.Path != "/login" && context.Request.Path != "/auth" && context.Request.Path != "/logout" && context.Request.Path != "/error" && context.Request.Path != "/developers")
                {
                    context.Response.Redirect("/login");
                }
                else
                {
                    await _next.Invoke(context);
                }
            }

            else
            {
                if(Coockies["AccessToken"] == db.Users
                                                .Where(u => u.VkId == int.Parse(Coockies["UserData"]))
                                                .First().AccessToken)
                {
                    int VkId = int.Parse(context.Request.Cookies["UserData"]);
                    User User = await db.Users.Where(u => u.VkId == VkId).FirstAsync();
                    bool isprofilefull = User.Course != 0 && User.Group != 0 && User.Description != "" ? false : true;
                    if(isprofilefull && context.Request.Path != "/profile" && context.Request.Path != "/logout")
                    {
                        context.Response.Redirect("/profile");
                    }
                    else
                    {
                        await _next.Invoke(context);
                    }
                }
                else
                {
                    context.Response.Cookies.Delete("UserData");
                    context.Response.Cookies.Delete("AccessToken");
                    if (context.Request.Path != "/login" && context.Request.Path != "/auth" && context.Request.Path != "/logout" && context.Request.Path != "/error" && context.Request.Path != "/developers")
                    {
                        context.Response.Redirect("/login");
                    }
                }
            }
                
        }
    }
}
