using Etch.OrchardCore.Lever.ViewModels;

namespace Etch.OrchardCore.Lever.Workflows.ViewModels
{
    public class LeverPostingNotificationViewModel
    {
        public string ErrorMessage { get; set; }
        public bool IsSuccessful { get; set; }
        public LeverPostingApplyViewModel LeverPostingApplyViewModel { get; set; }
    }
}
