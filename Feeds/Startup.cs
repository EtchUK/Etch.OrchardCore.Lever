using Etch.OrchardCore.Lever.Api.Services;
using Etch.OrchardCore.Lever.Services;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;

namespace Etch.OrchardCore.Lever.Feeds
{
    [Feature("Etch.OrchardCore.Lever.Feeds")]
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddHttpClient();

            services.AddScoped<IPostingApiService, PostingApiService>();
            services.AddScoped<ILeverPostingService, LeverPostingService>();
            services.AddScoped<ILeverPostingFeedService, LeverPostingFeedService>();
        }
    }
}
