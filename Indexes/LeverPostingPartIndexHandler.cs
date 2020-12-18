﻿using System.Threading.Tasks;
using Etch.OrchardCore.Lever.Api.Models.Dto;
using Etch.OrchardCore.Lever.Models;
using Newtonsoft.Json;
using OrchardCore.Indexing;
using OrchardCore.Liquid.Services;

namespace Etch.OrchardCore.Lever.Indexes
{
    public class LeverPostingPartIndexHandler : ContentPartIndexHandler<LeverPostingPart>
    {
        public override Task BuildIndexAsync(LeverPostingPart part, BuildPartIndexContext context)
        {
            var slugService = new SlugService();
            var options = context.Settings.ToOptions()
                | DocumentIndexOptions.Analyze | DocumentIndexOptions.Store
                ;

            var posting = JsonConvert.DeserializeObject<Posting>(part.Data);
            context.DocumentIndex.Set($"{nameof(LeverPostingPart)}.Text", posting.Text, options);
            context.DocumentIndex.Set($"{nameof(LeverPostingPart)}.Team", posting.Categories.Team, options);
            context.DocumentIndex.Set($"{nameof(LeverPostingPart)}.Team.Slug", slugService.Slugify(posting.Categories.Team), options);
            context.DocumentIndex.Set($"{nameof(LeverPostingPart)}.Location", posting.Categories.Location, options);
            context.DocumentIndex.Set($"{nameof(LeverPostingPart)}.Location.Slug", slugService.Slugify(posting.Categories.Location), options);
            context.DocumentIndex.Set($"{nameof(LeverPostingPart)}.Commitment", posting.Categories.Commitment, options);
            context.DocumentIndex.Set($"{nameof(LeverPostingPart)}.Commitment.Slug", slugService.Slugify(posting.Categories.Commitment), options);

            return Task.CompletedTask;
        }

    }
}
