using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Etch.OrchardCore.Lever.Api.Models.Dto;
using Etch.OrchardCore.Lever.Api.Services;
using Etch.OrchardCore.Lever.Extensions;
using Etch.OrchardCore.Lever.Models;
using Etch.OrchardCore.Lever.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Autoroute.Models;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Records;
using OrchardCore.Entities;
using OrchardCore.Environment.Shell;
using OrchardCore.Liquid;
using OrchardCore.Settings;
using YesSql;

namespace Etch.OrchardCore.Lever.Services
{
    public class LeverPostingService : ILeverPostingService
    {
        #region Dependancies

        public ILogger Logger { get; set; } = new NullLogger();
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly ILiquidTemplateManager _liquidTemplateManager;
        private readonly IPostingApiService _postingApiService;
        private readonly ISession _session;
        private readonly IShellHost _shellHost;
        private readonly ShellSettings _shellSettings;
        private readonly ISiteService _siteService;

        #endregion Dependancies

        #region Constructor

        public LeverPostingService(IContentDefinitionManager contentDefinitionManager, ILiquidTemplateManager liquidTemplateManager, IPostingApiService postingApiService, ISession session, IShellHost shellHost, ShellSettings shellSettings, ISiteService siteService)
        {
            _contentDefinitionManager = contentDefinitionManager;
            _liquidTemplateManager = liquidTemplateManager;
            _postingApiService = postingApiService;
            _session = session;
            _shellSettings = shellSettings;
            _shellHost = shellHost;
            _siteService = siteService;
        }

        #endregion

        #region Implementation

        public async Task<IList<ContentItem>> GetFromAPICreateUpdate()
        {
            var siteSettings = await _siteService.GetSiteSettingsAsync();
            var postings = await _postingApiService.GetPostings(siteSettings.As<LeverSettings>().Site);

            return await CreateUpdateAsync(postings);
        }

        private async Task<IList<ContentItem>> CreateUpdateAsync(IList<Posting> postings)
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
                var contentItem = postingContentItems.Where(x => x.As<LeverPostingPart>().LeverId == posting.Id).SingleOrDefault();

                // If not already exists create a new one
                if (contentItem == null)
                {
                    contentItems.Add(await CreateAsync(contentManager, posting));
                    continue;
                }

                contentItems.Add(await UpdateAsync(contentManager, contentItem, posting));
            }


            return contentItems;
        }

        public async Task<List<ContentItem>> GetAllAsync()
        {
            var contentItems = await _session.Query<ContentItem>()
                                      .With<ContentItemIndex>(x => x.Published && x.ContentType == Constants.Lever.ContentType)
                                      .ListAsync();

            return contentItems.ToList();
        }

        #endregion

        #region Private

        private async Task<ContentItem> CreateAsync(IContentManager contentManager, Posting posting)
        {
            var contentItem = await contentManager.NewAsync(Constants.Lever.ContentType);
            contentItem.DisplayText = posting.Text;
            contentItem.SetLeverPostingPart(posting);

            var autoroutePart = contentItem.As<AutoroutePart>();
            autoroutePart.Path = ToUrlSlug(posting.Text);
            contentItem.Apply(nameof(AutoroutePart), autoroutePart);

            ContentExtensions.Apply(contentItem, contentItem);

            await contentManager.CreateAsync(contentItem);
            await contentManager.PublishAsync(contentItem);

            return contentItem;
        }

        private async Task<ContentItem> UpdateAsync(IContentManager contentManager, ContentItem contentItem, Posting posting)
        {
            contentItem.DisplayText = posting.Text;
            contentItem.SetLeverPostingPart(posting);

            ContentExtensions.Apply(contentItem, contentItem);

            await contentManager.UpdateAsync(contentItem);

            return contentItem;
        }

        private async Task RemoveAsync(IContentManager contentManager, IList<ContentItem> contentItems)
        {
            foreach (var contentItem in contentItems)
            {
                await contentManager.RemoveAsync(contentItem);
            }
        }

        public static string ToUrlSlug(string value)
        {
            //First to lower case
            value = value.ToLowerInvariant();

            //Replace spaces
            value = Regex.Replace(value, @"\s", "-", RegexOptions.Compiled);

            //Remove invalid chars
            value = Regex.Replace(value, @"[^a-z0-9\s-_]", "", RegexOptions.Compiled);

            //Trim dashes from end
            value = value.Trim('-', '_');

            //Replace double occurences of - or _
            value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);

            return value;
        }


        #endregion
    }

    public interface ILeverPostingService
    {
        Task<IList<ContentItem>> GetFromAPICreateUpdate();
    }
}
