using FilesManager.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace FilesManager.API.Filters
{
    public class AuthorizationFilter : ActionFilterAttribute
    {
        private const string API_KEY = "api_key";
        private readonly AuthSettings _authSettings;

        public AuthorizationFilter(IOptions<AuthSettings> authSettings)
        {
            _authSettings = authSettings.Value;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.Headers.ContainsKey(API_KEY))
            {
                if (_authSettings.ApiKey == filterContext.HttpContext.Request.Headers[API_KEY].ToString())
                {
                    return;
                }
            }

            filterContext.Result = new UnauthorizedResult();
        }
    }
}
