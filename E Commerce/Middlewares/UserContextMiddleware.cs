using System.Security.Claims;

namespace E_Commerce.Middlewares
{
    public class UserContextMiddleware
    {
        private readonly RequestDelegate _next;

        public UserContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var userId = context.User.Claims.FirstOrDefault(n => n.Type == ClaimTypes.NameIdentifier);

                if (userId != null)
                {
                    var user_id = userId.Value;

                    context.Items["UserId"] = user_id;
                }
            }

            await _next(context);
        }
    }
}
