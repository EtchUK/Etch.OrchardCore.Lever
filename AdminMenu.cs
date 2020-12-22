using Microsoft.Extensions.Localization;
using OrchardCore.Modules;
using OrchardCore.Navigation;
using System;
using System.Threading.Tasks;

namespace Etch.OrchardCore.Lever
{
    [Feature("Etch.OrchardCore.Lever")]
    public class AdminMenu : INavigationProvider
    {
        public AdminMenu(IStringLocalizer<AdminMenu> localizer)
        {
            T = localizer;
        }

        public IStringLocalizer T { get; set; }

        public Task BuildNavigationAsync(string name, NavigationBuilder builder)
        {
            if (!string.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
            {
                return Task.CompletedTask;
            }

            builder
                .Add(T["Configuration"], configuration => configuration
                    .Add(T["Lever"], settings => settings
                        .Add(T["Api"], T["Api"], layers => layers
                            .Action("Index", "Admin", new { area = "OrchardCore.Settings", groupId = Constants.GroupId })
                            .Permission(Permissions.ManageLeverSettings)
                            .LocalNav()
                        )));

            return Task.CompletedTask;
        }
    }
}
