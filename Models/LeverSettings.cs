namespace Etch.OrchardCore.Lever.Models
{
    public class LeverSettings
    {
        public string ApiKey { get; set; }
        public string Site { get; set; }
        public string SuccessUrl { get; set; }
        public string FormId { get; set; }

        public string[] Locations { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(ApiKey) && !string.IsNullOrWhiteSpace(Site);
        }
    }
}
