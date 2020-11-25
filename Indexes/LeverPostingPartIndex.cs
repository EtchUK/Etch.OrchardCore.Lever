using System;
using Etch.OrchardCore.Lever.Models;
using OrchardCore.ContentManagement;
using YesSql.Indexes;

namespace Etch.OrchardCore.Lever.Indexes
{
    public class LeverPostingPartIndex : MapIndex
    {
        public string LeverId { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class LeverPostingPartIndexProvider : IndexProvider<ContentItem>
    {
        public override void Describe(DescribeContext<ContentItem> context)
        {
            context.For<LeverPostingPartIndex>()
                .Map(contentItem =>
                {
                    if (!Constants.Lever.ContentType.Equals(contentItem.ContentType))
                    {
                        return null;
                    }

                    var part = contentItem.As<LeverPostingPart>();
                    if (part != null)
                    {
                        return new LeverPostingPartIndex
                        {
                            LeverId = part.LeverId,
                            UpdatedAt = part.UpdatedAt
                        };
                    }

                    return null;
                });
        }
    }

}
