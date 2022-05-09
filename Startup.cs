using Etch.OrchardCore.Lever.Api.Models.Dto;
using Etch.OrchardCore.Lever.Api.Services;
using Etch.OrchardCore.Lever.Drivers;
using Etch.OrchardCore.Lever.Filters;
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
using OrchardCore.Liquid;
using OrchardCore.Modules;
using OrchardCore.Navigation;
using OrchardCore.Settings;
using YesSql.Indexes;

namespace Etch.OrchardCore.Lever
{
    [Feature("Etch.OrchardCore.Lever")]
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton<IIndexProvider, LeverPostingPartIndexProvider>();

            services.AddContentPart<LeverPostingPart>()
                    .UseDisplayDriver<LeverPostingPartDisplayDriver>();

            services.AddScoped<IPostingApiService, PostingApiService>();
            services.AddScoped<ILeverPostingService, LeverPostingService>();
            services.AddScoped<INavigationProvider, AdminMenu>();
            services.AddScoped<IDisplayDriver<ISite>, LeverSettingsDisplayDriver>();
            services.AddScoped<IContentPartIndexHandler, LeverPostingPartIndexHandler>();
            services.AddScoped<IDataMigration, Migrations>();

            services.AddLiquidFilter<CommitmentOptionsFilter>("lever_commitment_options");
            services.AddLiquidFilter<LocationOptionsFilter>("lever_location_options");
            services.AddLiquidFilter<TeamOptionsFilter>("lever_team_options");

            services.AddHttpClient();

            services.Configure<TemplateOptions>(o =>
            {
                o.MemberAccessStrategy.Register<LeverPostingPartViewModel>();
                o.MemberAccessStrategy.Register<LeverPostingApplyViewModel>();
                o.MemberAccessStrategy.Register<CustomQuestions>();
                o.MemberAccessStrategy.Register<Posting>();
                o.MemberAccessStrategy.Register<PostingCategories>();
                o.MemberAccessStrategy.Register<PostingLists>();
            });
        }
    }
}