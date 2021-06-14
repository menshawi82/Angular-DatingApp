using System;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {

         private readonly DataContext _context;
      

        public LogUserActivity(DataContext context)
        {
            _context = context;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

            var userId = resultContext.HttpContext.User.GetUserId();

            // var repo = resultContext.HttpContext.RequestServices.GetService<IUserRepository>();
            // var user = await repo.GetUserByIdAsync(userId);
             var user = await _context.Users.FindAsync(userId);

            user.LastActive = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }
} 