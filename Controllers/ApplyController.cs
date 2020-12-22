using System.Linq;
using System.Threading.Tasks;
using Etch.OrchardCore.Lever.Api.Models.Dto;
using Etch.OrchardCore.Lever.Api.Services;
using Etch.OrchardCore.Lever.Extensions;
using Etch.OrchardCore.Lever.Models;
using Etch.OrchardCore.Lever.Services;
using Etch.OrchardCore.Lever.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Routing;
using OrchardCore.Entities;
using OrchardCore.Modules;
using OrchardCore.Settings;

namespace Etch.OrchardCore.Lever.Controllers
{
    [Feature("Etch.OrchardCore.Lever")]
    public class ApplyController : Controller
    {
        #region Dependancies

        private readonly IAutorouteEntries _autorouteEntries;
        private readonly ILeverPostingService _leverPostingService;
        private readonly IPostingApiService _postingApiService;
        private readonly ISiteService _siteService;

        #endregion Dependancies

        #region Constructor

        public ApplyController(IAutorouteEntries autorouteEntries, ILeverPostingService leverPostingService, IPostingApiService postingApiService, ISiteService siteService)
        {
            _autorouteEntries = autorouteEntries;
            _leverPostingService = leverPostingService;
            _postingApiService = postingApiService;
            _siteService = siteService;
        }

        #endregion

        [HttpPost]
        [Route("lever-apply")]
        public async Task<IActionResult> Index(LeverPostingApplyViewModel model)
        {
            var settings = (await _siteService.GetSiteSettingsAsync()).As<LeverSettings>();
            var referer = Request.Headers["Referer"].ToString();

            if (!ModelState.IsValid)
            {
                return new RedirectResult(referer);
            }

            _autorouteEntries.TryGetEntryByPath("/" + referer.Split('/').Last(), out var contentItem);

            if (contentItem == null)
            {
                return new BadRequestResult();
            }

            var posting = await _leverPostingService.GetById(contentItem.ContentItemId);

            if (posting == null)
            {
                return new BadRequestResult();
            }

            model.PostingId = JsonConvert.DeserializeObject<Posting>(posting.GetLeverPostingPart().Data).Id;
            model.UpdateCards(HttpContext.Request.Form);

            var result = await _postingApiService.Apply(settings, model);

            if (!result.Ok)
            {
                ModelState.AddModelError("error", result.Error);
                return new RedirectResult(referer);
            }

            return new RedirectResult($"{settings.SuccessUrl}?applicationId={result.ApplicationId}" ?? "/");
        }
    }
}
