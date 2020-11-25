namespace Etch.OrchardCore.Lever.Api.Models.Dto
{
    public class Posting
    {
        public string AdditionalPlain { get; set; }

        public string Additional { get; set; }

        public long CreatedAt { get; set; }

        public string DescriptionPlain { get; set; }

        public string Description { get; set; }

        public string Id { get; set; }

        public string Text { get; set; }

        public string HostedUrl { get; set; }

        public string ApplyUrl { get; set; }

        public PostingCategories Categories { get; set; }

        public PostingLists[] Lists { get; set; }
    }

    public class PostingCategories
    {
        public string Team { get; set; }
        public string Location { get; set; }
        public string Commitment { get; set; }
    }

    public class PostingLists
    {
        public string Text { get; set; }
        public string Content { get; set; }
    }
}
