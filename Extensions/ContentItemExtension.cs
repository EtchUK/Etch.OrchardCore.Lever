using Etch.OrchardCore.Lever.Api.Models.Dto;
using Etch.OrchardCore.Lever.Models;
using Newtonsoft.Json;
using OrchardCore.ContentManagement;

namespace Etch.OrchardCore.Lever.Extensions
{
    public static class ContentItemExtension
    {
        public static void SetLeverPostingPart(this ContentItem contentItem, Posting posting)
        {
            var leverPostingPart = contentItem.As<LeverPostingPart>();
            leverPostingPart.LeverId = posting.Id;
            leverPostingPart.UpdatedAt = posting.UpdatedAt.UnixTimeStampToDateTime();
            leverPostingPart.Data = JsonConvert.SerializeObject(posting);
            leverPostingPart.Apply();


            contentItem.Apply(nameof(LeverPostingPart), leverPostingPart);
        }
    }
}
