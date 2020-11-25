using Etch.OrchardCore.Lever.Workflow.ViewModels;
using Etch.OrchardCore.Lever.Workflows.Activities;
using OrchardCore.Workflows.Display;

namespace Etch.OrchardCore.Lever.Workflows.Drivers
{
    public class LeverPostingTaskDisplay : ActivityDisplayDriver<LeverPostingTask, LeverPostingTaskViewModel>
    {
        protected override void EditActivity(LeverPostingTask posting, LeverPostingTaskViewModel model)
        {
        }

        protected override void UpdateActivity(LeverPostingTaskViewModel model, LeverPostingTask posting)
        {
        }
    }
}
