using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OrchardCore.BackgroundTasks;

namespace Etch.OrchardCore.Lever.Services
{
    [BackgroundTask(Schedule = "0 * * * *", Description = "Getting and updating lever postings.")]
    public class LeverPostingBackgroundTask : IBackgroundTask
    {
        #region Dependencies

        private readonly ILeverPostingService _leverPostingService;
        private readonly ILogger<LeverPostingBackgroundTask> _logger;

        #endregion

        #region Constructor

        public LeverPostingBackgroundTask(ILeverPostingService leverPostingService, ILogger<LeverPostingBackgroundTask> logger)
        {
            _logger = logger;
            _leverPostingService = leverPostingService;
        }

        #endregion

        #region Implementation

        public async Task DoWorkAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Getting posts from Lever API");

            await _leverPostingService.GetFromAPICreateUpdate();
        }

        #endregion
    }
}
