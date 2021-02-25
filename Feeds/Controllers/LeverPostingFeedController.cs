using System.Threading.Tasks;
using System.Xml.Linq;
using Etch.OrchardCore.Lever.Services;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.Modules;

namespace Etch.OrchardCore.Lever.Feeds.Controllers
{
    [Feature("Etch.OrchardCore.Lever.Feeds")]
    public class LeverPostingFeedController : Controller
    {
        #region Dependancies

        private readonly ILeverPostingFeedService _leverPostingFeedService;

        #endregion Dependancies

        #region Constructor

        public LeverPostingFeedController(ILeverPostingFeedService leverPostingFeedService)
        {
            _leverPostingFeedService = leverPostingFeedService;
        }

        #endregion

        [Route("postings/feed/mcv")]
        public async Task<ActionResult> Mcv()
        {
            return new ContentResult
            {
                Content = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), await _leverPostingFeedService.CreateFeedAsync()).ToString(),
                ContentType = "text/xml",
                StatusCode = 200
            };
        }
    }
}
