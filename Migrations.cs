using System;
using System.Threading.Tasks;
using Etch.OrchardCore.Lever.Indexes;
using Etch.OrchardCore.Lever.Models;
using OrchardCore.Autoroute.Models;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Recipes.Services;
using OrchardCore.Title.Models;

namespace Etch.OrchardCore.Lever
{
    public class Migrations : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly IRecipeMigrator _recipeMigrator;

        public Migrations(IContentDefinitionManager contentDefinitionManager, IRecipeMigrator recipeMigrator)
        {
            _contentDefinitionManager = contentDefinitionManager;
            _recipeMigrator = recipeMigrator;
        }

        public async Task<int> CreateAsync()
        {
            _contentDefinitionManager.AlterPartDefinition(nameof(LeverPostingPart), part => part
                .WithDescription("Create a Lever posting content.")
                .WithDisplayName(Constants.Lever.DisplayName));

            _contentDefinitionManager.AlterTypeDefinition(Constants.Lever.ContentType, type => type
                .Draftable()
                .Versionable()
                .Listable()
                .Creatable()
                .Securable()
                .WithPart(nameof(TitlePart))
                .WithPart("AutoroutePart", part => part.WithSettings(new AutoroutePartSettings
                {
                    AllowCustomPath = true,
                    Pattern = "{{ Model.ContentItem | display_text | slugify }}"
                }))
                .WithPart(nameof(LeverPostingPart))
                .DisplayedAs(Constants.Lever.DisplayName));

            SchemaBuilder.CreateMapIndexTable(nameof(LeverPostingPartIndex), table => table
                .Column<string>("LeverId")
            );

            SchemaBuilder.AlterTable(nameof(LeverPostingPartIndex), table => table
                .CreateIndex("IDX_LeverPostingPartIndex_LeverId", "LeverId")
            );

            await _recipeMigrator.ExecuteAsync("create.recipe.json", this);

            return 1;
        }
    }
}
