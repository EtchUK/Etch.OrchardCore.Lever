using Etch.OrchardCore.Lever.Api.Models.Dto;
using Etch.OrchardCore.Lever.Models;
using Etch.OrchardCore.Lever.ViewModels;
using Newtonsoft.Json;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.Views;

namespace Etch.OrchardCore.Lever.Drivers
{
    public class LeverPostingPartDisplayDriver : ContentPartDisplayDriver<LeverPostingPart>
    {
        public override IDisplayResult Display(LeverPostingPart part, BuildPartDisplayContext context)
        {
            if (context.DisplayType == "SummaryAdmin")
            {
                return null;
            }

            return Initialize<LeverPostingPartViewModel>("LeverPostingPart", model =>
            {
                model.ContentItem = part.ContentItem;
                model.Posting = JsonConvert.DeserializeObject<Posting>(part.Data);
            })
            .Location("Content");
        }

        public override IDisplayResult Edit(LeverPostingPart part)
        {
            return Initialize<LeverPostingPartViewModel>("LeverPostingPart_Edit", model =>
            {
                model.Posting = JsonConvert.DeserializeObject<Posting>(part.Data);
            });
        }
    }
}
