using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Etch.OrchardCore.Lever.Api.Models.Dto;
using Etch.OrchardCore.Lever.Models;
using Etch.OrchardCore.Lever.ViewModels;
using Newtonsoft.Json;

namespace Etch.OrchardCore.Lever.Api.Services
{
    public class PostingApiService : IPostingApiService
    {
        #region Constants

        private const string URL = "https://api.lever.co/v0/postings/";

        #endregion

        #region Dependencies

        private readonly IHttpClientFactory _clientFactory;
        public ILogger Logger { get; set; } = new NullLogger();

        #endregion Dependencies

        #region Constructor

        public PostingApiService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        #endregion

        #region Implementation

        public async Task<IList<Posting>> GetPostings(LeverSettings settings)
        {
            if (settings == null || !settings.IsValid())
            {
                Logger.Error("Unable to fetch posting from API because `ApiKey` or `Site` is not defined in settings.");
                return null;
            }

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{URL}{settings.Site}");
                var client = _clientFactory.CreateClient();

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<IList<Posting>>(await response.Content.ReadAsStringAsync()).Where(x => FilterPostings(settings, x)).ToList();
                }
            }
            catch (Exception e)
            {
                Logger.Error(string.Format("{0}, Error retrieving data from API.", e));
            }

            return null;
        }

        public async Task<PostingResult> Apply(LeverSettings settings, LeverPostingApplyViewModel model)
        {
            if (settings == null || !settings.IsValid())
            {
                Logger.Error("Unable to submit application to API because `Site` or `ApiKey` is not defined in settings.");
                return null;
            }

            if (string.IsNullOrEmpty(model.PostingId))
            {
                Logger.Error("Please provide postingId.");
                return null;
            }

            try
            {
                var client = _clientFactory.CreateClient();
                var response = await client.PostAsync($"{URL}{settings.Site}/{model.PostingId}?key={settings.ApiKey}", model.ToDataFrom());

                return JsonConvert.DeserializeObject<PostingResult>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception e)
            {
                Logger.Error(string.Format("{0}, Error apply for posting id: {1}", e, model.PostingId));
            }

            return null;
        }

        #endregion

        #region Helper Methods

        private bool FilterPostings(LeverSettings settings, Posting posting)
        {
            if (settings.Locations == null || !settings.Locations.Any())
            {
                return true;
            }

            return settings.Locations.Any(x => string.Equals(x, posting.Categories.Location, StringComparison.OrdinalIgnoreCase));
        }

        #endregion
    }

    public interface IPostingApiService
    {
        Task<PostingResult> Apply(LeverSettings settings, LeverPostingApplyViewModel model);
        Task<IList<Posting>> GetPostings(LeverSettings settings);
    }
}
