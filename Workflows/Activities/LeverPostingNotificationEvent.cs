using System.Collections.Generic;
using Etch.OrchardCore.Lever.Workflows.ViewModels;
using Microsoft.Extensions.Localization;
using OrchardCore.Modules;
using OrchardCore.Workflows.Abstractions.Models;
using OrchardCore.Workflows.Activities;
using OrchardCore.Workflows.Helpers;
using OrchardCore.Workflows.Models;

namespace Etch.OrchardCore.Lever.Workflows.Activities
{
    [Feature("Etch.OrchardCore.Lever")]
    public class LeverPostingNotificationEvent : Activity, IEvent
    {
        #region Dependencies

        public IStringLocalizer T { get; }

        #endregion Dependencies

        #region Constructor

        public LeverPostingNotificationEvent(IStringLocalizer<LeverPostingNotificationEvent> localizer)
        {
            T = localizer;
        }

        #endregion Constructor

        #region Activity

        public override string Name => nameof(LeverPostingNotificationEvent);

        public override LocalizedString Category => T["Lever"];

        public override LocalizedString DisplayText => T["Lever posting apply event"];

        public override ActivityExecutionResult Execute(WorkflowExecutionContext workflowContext, ActivityContext activityContext)
        {
            var model = GetLeverPostingNotificationViewModelDetail(workflowContext);

            SetLeverPostingNotificationViewModelProperties(workflowContext, model);

            if (!model.IsSuccessful)
            {
                return Outcomes("Failed");
            }

            return Outcomes("Done");
        }

        public override IEnumerable<Outcome> GetPossibleOutcomes(WorkflowExecutionContext workflowContext, ActivityContext activityContext)
        {
            return Outcomes(T["Done"], T["Failed"]);
        }

        #endregion Activity

        #region Private

        private LeverPostingNotificationViewModel GetLeverPostingNotificationViewModelDetail(WorkflowExecutionContext workflowExecutionContext)
        {
            return workflowExecutionContext.Input.GetValue<LeverPostingNotificationViewModel>(nameof(LeverPostingNotificationViewModel)) ??
                workflowExecutionContext.Properties.GetValue<LeverPostingNotificationViewModel>(nameof(LeverPostingNotificationViewModel));
        }

        private void SetLeverPostingNotificationViewModelProperties(WorkflowExecutionContext workflowContext, LeverPostingNotificationViewModel posting)
        {
            foreach (var property in posting.GetType().GetProperties())
            {
                workflowContext.Properties[property.Name] = property.GetValue(posting);
            }
        }

        #endregion
    }
}
