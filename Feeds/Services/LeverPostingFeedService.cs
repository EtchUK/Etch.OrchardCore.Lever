using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Etch.OrchardCore.Lever.Api.Models.Dto;
using Etch.OrchardCore.Lever.Extensions;
using Etch.OrchardCore.Lever.Feeds.Extensions;
using Newtonsoft.Json;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.Settings;

namespace Etch.OrchardCore.Lever.Services
{
    public class LeverPostingFeedService : ILeverPostingFeedService
    {
        #region Dependancies

        private readonly ILeverPostingService _leverPostingService;
        private readonly ISiteService _siteService;

        #endregion Dependancies

        #region Constructor

        public LeverPostingFeedService(ILeverPostingService leverPostingService, ISiteService siteService)
        {
            _leverPostingService = leverPostingService;
            _siteService = siteService;
        }

        #endregion

        #region Implementation

        public async Task<XElement> CreateFeedAsync()
        {
            var postings = await _leverPostingService.GetAllAsync();
            var settings = await _siteService.GetSiteSettingsAsync();

            var jobs = new XElement("jobs");

            foreach (var posting in postings)
            {
                var job = new XElement("job");
                var jobInfo = new XElement("job_info");
                var location = new XElement("location");

                var postingPart = JsonConvert.DeserializeObject<Posting>(posting.GetLeverPostingPart().Data);

                job.Add(new XElement("site_id", new XCData(settings.GetSiteIdValue())));

                jobInfo.Add(new XElement("company", new XCData(settings.GetSiteNameValue())));
                jobInfo.Add(new XElement("description", new XCData($"{postingPart.DescriptionPlain} {GetLists(postingPart.Lists)} {postingPart.AdditionalPlain}")));
                jobInfo.Add(new XElement("name", new XCData(postingPart.Text)));
                jobInfo.Add(new XElement("position", new XCData(postingPart.Categories.Team)));

                location.Add(new XElement("country", new XCData(settings.GetCountryValue())));
                location.Add(new XElement("state", new XCData(postingPart.Categories.Location)));

                job.Add(jobInfo);
                job.Add(location);
                jobs.Add(job);
            }


            return jobs;
        }

        #endregion

        #region Private

        private string GetLists(PostingLists[] lists)
        {
            var text = string.Empty;

            foreach (var list in lists)
            {
                text = $"{text} {list.Text} {Environment.NewLine}";
                text = $"{text} {ReplaceLists(list.Content)} {Environment.NewLine}{Environment.NewLine}";
            }

            return text;
        }

        private string ReplaceLists(string text)
        {
            return Regex.Replace(text.Replace("</li>", Environment.NewLine), "<[^>]*(>|$)", string.Empty);
        }

        #endregion
    }

    public interface ILeverPostingFeedService
    {
        Task<XElement> CreateFeedAsync();
    }
}
