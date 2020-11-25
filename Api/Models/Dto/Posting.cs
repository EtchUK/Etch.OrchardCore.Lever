namespace Etch.OrchardCore.Lever.Api.Models.Dto
{
    public class PostingData
    {
        public Posting[] Data { get; set; }
    }

    public class Posting
    {
        public string Id { get; set; }

        public string Text { get; set; }

        public long CreatedAt { get; set; }

        public long UpdatedAt { get; set; }

        public string User { get; set; }

        public string Owner { get; set; }

        public string HiringManager { get; set; }

        public string Confidentiality { get; set; }

        public PostingCategories Categories { get; set; }

        public PostingContent Content { get; set; }

        public string[] Tags { get; set; }

        public string State { get; set; }

        public string[] DistributionChannels { get; set; }

        public string ReqCode { get; set; }

        public string[] RequisitionCodes { get; set; }

        public PostingUrls Urls { get; set; }
    }

    public class PostingCategories
    {
        public string Team { get; set; }
        public string Department { get; set; }
        public string Location { get; set; }
        public string Commitment { get; set; }
        public string Level { get; set; }
    }

    public class PostingContent
    {
        public string Description { get; set; }
        public string DescriptionHtml { get; set; }
        public string Closing { get; set; }
        public PostingContentLists[] Lists { get; set; }
        public string ClosingHtml { get; set; }
    }

    public class PostingContentLists
    {
        public string Text { get; set; }
        public string Content { get; set; }
    }

    public class PostingUrls
    {
        public string List { get; set; }
        public string Show { get; set; }
        public string Apply { get; set; }
    }
}
