using Etch.OrchardCore.Lever.Workflows.Activities;
using Etch.OrchardCore.Lever.Workflows.ViewModels;
using OrchardCore.Workflows.Display;

namespace Etch.OrchardCore.Lever.Workflows.Drivers
{

    public class LeverPostingNotificationDisplay : ActivityDisplayDriver<LeverPostingNotificationEvent, LeverPostingNotificationViewModel>
    {
        protected override void EditActivity(LeverPostingNotificationEvent notificationEvent, LeverPostingNotificationViewModel model)
        {
        }

        protected override void UpdateActivity(LeverPostingNotificationViewModel model, LeverPostingNotificationEvent notificationEvent)
        {
        }
    }
}
