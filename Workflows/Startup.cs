﻿using Etch.OrchardCore.Lever.Workflows.Activities;
using Etch.OrchardCore.Lever.Workflows.Drivers;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using OrchardCore.Workflows.Helpers;

namespace Etch.OrchardCore.Lever.Workflows
{
    [RequireFeatures("OrchardCore.Workflows")]
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddActivity<LeverPostingTask, LeverPostingTaskDisplay>();
        }
    }
}
