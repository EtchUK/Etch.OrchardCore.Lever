using System.Threading.Tasks;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using OrchardCore.Recipes.Services;

namespace Etch.OrchardCore.Lever.Feeds
{
    [Feature("Etch.OrchardCore.Lever.Feeds")]
    public class Migrations : DataMigration
    {
        private readonly IRecipeMigrator _recipeMigrator;

        public Migrations(IRecipeMigrator recipeMigrator)
        {
            _recipeMigrator = recipeMigrator;
        }

        public async Task<int> CreateAsync()
        {
            await _recipeMigrator.ExecuteAsync("custom-settings.recipe.json", this);

            return 1;
        }
    }
}
