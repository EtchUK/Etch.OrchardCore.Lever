using System.Threading.Tasks;
using Etch.OrchardCore.Lever.Models;
using Etch.OrchardCore.Lever.Services;
using Etch.OrchardCore.Lever.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using OrchardCore.DisplayManagement.Entities;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Settings;

namespace Etch.OrchardCore.Lever.Drivers
{
    public class LeverSettingsDisplayDriver : SectionDisplayDriver<ISite, LeverSettings>
    {
        #region Dependencies

        private readonly IAuthorizationService _authorizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region Constructor

        public LeverSettingsDisplayDriver(IAuthorizationService authorizationService, IHttpContextAccessor httpContextAccessor, ILeverPostingService leverPostingService)
        {
            _authorizationService = authorizationService;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Overrides

        public override async Task<IDisplayResult> EditAsync(LeverSettings section, BuildEditorContext context)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (!await _authorizationService.AuthorizeAsync(user, Permissions.ManageLeverSettings))
            {
                return null;
            }

            return Initialize<LeverSettingsViewModel>("LeverSettings_Edit", model =>
            {
                model.ApiKey = section.ApiKey;
                model.Site = section.Site;
                model.SuccessUrl = section.SuccessUrl;
                model.FormId = section.FormId;
            }).Location("Content:3").OnGroup(Constants.GroupId);
        }

        public override async Task<IDisplayResult> UpdateAsync(LeverSettings section, BuildEditorContext context)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (!await _authorizationService.AuthorizeAsync(user, Permissions.ManageLeverSettings))
            {
                return null;
            }

            if (context.GroupId == Constants.GroupId)
            {
                var model = new LeverSettingsViewModel();

                await context.Updater.TryUpdateModelAsync(model, Prefix);

                section.ApiKey = model.ApiKey;
                section.Site = model.Site;
                section.SuccessUrl = model.SuccessUrl;
                section.FormId = model.FormId;
            }

            return await EditAsync(section, context);
        }

        #endregion
    }
}