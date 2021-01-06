using Etch.OrchardCore.Lever.Api.Models.Dto;
using Etch.OrchardCore.Lever.Models;
using Newtonsoft.Json;
using OrchardCore.ContentManagement;

namespace Etch.OrchardCore.Lever.Extensions
{
    public static class ContentItemExtension
    {
        public static bool ComparePostingPart(this ContentItem contentItem, Posting posting)
        {
            var part = contentItem.As<LeverPostingPart>();

            if (part == null)
            {
                return false;
            }

            return contentItem.As<LeverPostingPart>().Data == JsonConvert.SerializeObject(posting);
        }

        public static LeverPostingPart GetLeverPostingPart(this ContentItem contentItem)
        {
            return contentItem.As<LeverPostingPart>();
        }

        public static void SetLeverPostingPart(this ContentItem contentItem, Posting posting)
        {
            var part = contentItem.As<LeverPostingPart>();

            if (part == null)
            {
                return;
            }

            part.LeverId = posting.Id;
            part.Data = JsonConvert.SerializeObject(posting);
            part.Apply();

            contentItem.Apply(nameof(LeverPostingPart), part);
        }
    }
}
