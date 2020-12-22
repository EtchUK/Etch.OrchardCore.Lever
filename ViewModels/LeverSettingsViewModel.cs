namespace Etch.OrchardCore.Lever.ViewModels
{
    public class LeverSettingsViewModel
    {
        public string ApiKey { get; set; }
        public string FormId { get; set; }
        public string[] Locations { get; set; }
        public string LocationsJson { get; set; }
        public string Site { get; set; }
        public string SuccessUrl { get; set; }
    }
}
