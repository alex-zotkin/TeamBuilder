using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamBuilder.Models;

namespace TeamBuilder.Middlewares
{
    public class IsUserInfoFull
    {
        private readonly RequestDelegate _next;
        public IsUserInfoFull(RequestDelegate del)
        {
            _next = del;
        }

        public async Task InvokeAsync(HttpContext context, DataBaseContext db)
        {
            int VkId = int.Parse(context.Request.Cookies["UserData"]);
            User User = await db.Users.Where(u => u.VkId == VkId).FirstAsync();
            bool isprofilefull = User.Course != 0 && User.Group != 0 && User.Description != "" ? false : true;
            if (isprofilefull && context.Request.Path != "/profile" && context.Request.Path != "/logout")
            {
                context.Response.Redirect("/profile");
            }
            else
            {
                await _next.Invoke(context);
            }
        }
    }
}
