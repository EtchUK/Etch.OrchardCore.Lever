using Etch.OrchardCore.Lever.Api.Models.Dto;
using Etch.OrchardCore.Lever.Api.Services;
using Etch.OrchardCore.Lever.Drivers;
using Etch.OrchardCore.Lever.Indexes;
using Etch.OrchardCore.Lever.Models;
using Etch.OrchardCore.Lever.Services;
using Etch.OrchardCore.Lever.ViewModels;
using Fluid;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.BackgroundTasks;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.Data.Migration;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.Indexing;
using OrchardCore.Modules;
using OrchardCore.Navigation;
using OrchardCore.Settings;
using YesSql.Indexes;

namespace Etch.OrchardCore.Lever
{
    public class Startup : StartupBase
    {
        static Startup()
        {
            TemplateContext.GlobalMemberAccessStrategy.Register<LeverPostingPartViewModel>();
            TemplateContext.GlobalMemberAccessStrategy.Register<Posting>();
            TemplateContext.GlobalMemberAccessStrategy.Register<PostingCategories>();
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton<IPostingApiService, PostingApiService>();
            services.AddSingleton<IIndexProvider, LeverPostingPartIndexProvider>();

            services.AddContentPart<LeverPostingPart>()
                    .UseDisplayDriver<LeverPostingPartDisplayDriver>();

            services.AddScoped<IBackgroundTask, LeverPostingBackgroundTask>();
            services.AddScoped<ILeverPostingService, LeverPostingService>();
            services.AddScoped<INavigationProvider, AdminMenu>();
            services.AddScoped<IDisplayDriver<ISite>, LeverSettingsDisplayDriver>();
            services.AddScoped<IContentPartIndexHandler, LeverPostingPartIndexHandler>();
            services.AddScoped<IDataMigration, Migrations>();
            services.AddHttpClient();
        }
    }
}