using Castle.Core.Logging;
using Etch.OrchardCore.Lever.Api.Models.Dto;
using Etch.OrchardCore.Lever.Api.Services;
using Etch.OrchardCore.Lever.Extensions;
using Etch.OrchardCore.Lever.Models;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Autoroute.Models;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Records;
using OrchardCore.ContentManagement.Routing;
using OrchardCore.Entities;
using OrchardCore.Environment.Shell;
using OrchardCore.Liquid;
using OrchardCore.Settings;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YesSql;

namespace Etch.OrchardCore.Lever.Services
{
    public class LeverPostingService : ILeverPostingService
    {
        #region Dependancies

        public ILogger Logger { get; set; } = new NullLogger();

        private readonly IAutorouteEntries _autorouteEntries;
        private readonly IPostingApiService _postingApiService;
        private readonly ISession _session;
        private readonly IShellHost _shellHost;
        private readonly ShellSettings _shellSettings;
        private readonly ISiteService _siteService;
        private readonly ISlugService _slugService;

        #endregion Dependancies

        #region Constructor

        public LeverPostingService(IAutorouteEntries autorouteEntries, IPostingApiService postingApiService, ISession session, IShellHost shellHost, ShellSettings shellSettings, ISiteService siteService, ISlugService slugService)
        {
            _autorouteEntries = autorouteEntries;
            _postingApiService = postingApiService;
            _session = session;
            _shellSettings = shellSettings;
            _shellHost = shellHost;
            _siteService = siteService;
            _slugService = slugService;
        }

        #endregion

        #region Implementation

        public async Task<IList<ContentItem>> GetFromAPICreateUpdate()
        {
            var siteSettings = await _siteService.GetSiteSettingsAsync();

            _postingApiService.Init(siteSettings.As<LeverSettings>().Site, siteSettings.As<LeverSettings>().ApiKey);

            return await CreateUpdateAsync(await _postingApiService.GetPostings(), siteSettings.As<LeverSettings>().FormId);
        }

        private async Task<IList<ContentItem>> CreateUpdateAsync(IList<Posting> postings, string formId)
        {
            var contentItems = new List<ContentItem>();

            using var scope = await _shellHost.GetScopeAsync(_shellSettings);
            var contentManager = scope.ServiceProvider.GetRequiredService<IContentManager>();

            var postingContentItems = await GetAllAsync();

            // Remove old postings
            await RemoveAsync(contentManager, postingContentItems.Where(x => !postings.Any(y => y.Id == x.As<LeverPostingPart>().LeverId)).ToList());

            // Add/Update posings
            foreach (var posting in postings)
            {
                var contentItem = postingContentItems.SingleOrDefault(x => x.As<LeverPostingPart>().LeverId == posting.Id);

                // If not already exists create a new one
                if (contentItem == null)
                {
                    contentItems.Add(await CreateAsync(contentManager, posting, formId));
                    continue;
                }

                contentItems.Add(await UpdateAsync(contentManager, contentItem, posting));
            }


            return contentItems;
        }

        public async Task<IEnumerable<ContentItem>> GetAllAsync()
        {
            return await _session.Query<ContentItem>()
                .With<ContentItemIndex>(x => x.Published && x.ContentType == Constants.Lever.ContentType)
                .ListAsync();
        }

        public async Task<ContentItem> GetById(string contentItemId)
        {
            return await _session.Query<ContentItem>()
                .With<ContentItemIndex>(x => x.Published && x.ContentType == Constants.Lever.ContentType && x.ContentItemId == contentItemId)
                .FirstOrDefaultAsync();
        }

        #endregion

        #region Private

        private async Task<ContentItem> CreateAsync(IContentManager contentManager, Posting posting, string formId)
        {
            var contentItem = await contentManager.NewAsync(Constants.Lever.ContentType);
            contentItem.DisplayText = posting.Text;
            contentItem.SetLeverPostingPart(posting);

            var autoroutePart = contentItem.As<AutoroutePart>();
            autoroutePart.Path = $"{_slugService.Slugify(posting.Text)}/{posting.Id}";
            contentItem.Apply(nameof(AutoroutePart), autoroutePart);

            var leverApply = contentItem.GetOrCreate<ContentPart>("LeverPosting");
            var contentPickerField = contentItem.GetOrCreate<ContentPickerField>("LeverApplyForm");
            contentPickerField.ContentItemIds = new string[] { formId };
            leverApply.Apply("LeverApplyForm", contentPickerField);
            contentItem.Apply("LeverPosting", leverApply);

            ContentExtensions.Apply(contentItem, contentItem);

            await contentManager.CreateAsync(contentItem);
            await contentManager.PublishAsync(contentItem);

            return contentItem;
        }

        private async Task RemoveAsync(IContentManager contentManager, IList<ContentItem> contentItems)
        {
            foreach (var contentItem in contentItems)
            {
                await contentManager.RemoveAsync(contentItem);
            }
        }

        private async Task<ContentItem> UpdateAsync(IContentManager contentManager, ContentItem contentItem, Posting posting)
        {
            contentItem.DisplayText = posting.Text;
            contentItem.SetLeverPostingPart(posting);

            ContentExtensions.Apply(contentItem, contentItem);

            await contentManager.UpdateAsync(contentItem);

            return contentItem;
        }


        #endregion
    }

    public interface ILeverPostingService
    {
        Task<IList<ContentItem>> GetFromAPICreateUpdate();
        Task<ContentItem> GetById(string contentItemId);
    }
}
