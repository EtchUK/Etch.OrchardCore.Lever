using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Etch.OrchardCore.Lever.Services;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using OrchardCore.Workflows.Abstractions.Models;
using OrchardCore.Workflows.Activities;
using OrchardCore.Workflows.Models;

namespace Etch.OrchardCore.Lever.Workflows.Activities
{
    public class LeverPostingTask : TaskActivity
    {
        #region Constants

        private const string OutcomeDone = "Done";
        private const string OutcomeFailed = "Failed";

        #endregion Constants

        #region Dependencies

        private readonly ILeverPostingService _leverPostingService;
        private readonly ILogger<LeverPostingTask> _logger;
        private IStringLocalizer T { get; }

        #endregion Dependencies

        #region Constructor

        public LeverPostingTask(ILeverPostingService leverPostingService, ILogger<LeverPostingTask> logger, IStringLocalizer<LeverPostingTask> stringLocalizer)
        {
            _leverPostingService = leverPostingService;
            _logger = logger;
            T = stringLocalizer;
        }

        #endregion Constructor

        #region Implementations

        public override string Name => nameof(LeverPostingTask);
        public override LocalizedString DisplayText => T["Getting and updating lever postings from API."];
        public override LocalizedString Category => T["Lever"];

        public override IEnumerable<Outcome> GetPossibleOutcomes(WorkflowExecutionContext workflowContext, ActivityContext activityContext)
        {
            return Outcomes(T[OutcomeDone], T[OutcomeFailed]);
        }

        public override async Task<ActivityExecutionResult> ExecuteAsync(WorkflowExecutionContext workflowContext, ActivityContext activityContext)
        {
            try
            {
                await _leverPostingService.GetFromAPICreateUpdate();

                return Outcomes(OutcomeDone);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                return Outcomes(OutcomeFailed);
            }
        }

        #endregion Implementations
    }
}