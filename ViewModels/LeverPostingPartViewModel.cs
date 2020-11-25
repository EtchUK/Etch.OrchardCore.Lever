using Etch.OrchardCore.Lever.Api.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrchardCore.ContentManagement;

namespace Etch.OrchardCore.Lever.ViewModels
{
    public class LeverPostingPartViewModel
    {
        public Posting Posting { get; set; }

        [BindNever]
        public ContentItem ContentItem { get; set; }
    }
}
