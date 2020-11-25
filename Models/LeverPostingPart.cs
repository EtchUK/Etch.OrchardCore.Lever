using System;
using OrchardCore.ContentManagement;

namespace Etch.OrchardCore.Lever.Models
{
    public class LeverPostingPart : ContentPart
    {
        public string LeverId { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Data { get; set; }
    }
}
