
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace InternetBanking.Core.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
        }
    }
}
