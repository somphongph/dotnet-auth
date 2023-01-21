using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace Domain.Filter
{
    public class ServiceAuthenFilter : IActionFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public ServiceAuthenFilter(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void OnActionExecuting(ActionExecutingContext objContext)
        {
            string appId = _httpContextAccessor?.HttpContext?.Request.Headers["X-App-Id"]!;
            string appKey = _httpContextAccessor?.HttpContext?.Request.Headers["X-App-Key"]!;

            string settingAppId = _configuration["AppAuthen:AppId"];
            string settingAppKey = _configuration["AppAuthen:AppKey"];

            if (!(appId.Equals(settingAppId) && appKey.Equals(settingAppKey)))
            {
                throw new UnauthorizedAccessException();
            }
        }

        public void OnActionExecuted(ActionExecutedContext objContext)
        {
        }
    }
}