using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Etch.OrchardCore.Lever.Api.Models.Dto;
using Etch.OrchardCore.Lever.ViewModels;
using Newtonsoft.Json;

namespace Etch.OrchardCore.Lever.Api.Services
{
    public class PostingApiService : IPostingApiService
    {
        #region Constants

        private const string URL = "https://api.lever.co/v0/postings/";
        private string site = null;
        private string api = null;

        #endregion

        #region Dependancies

        private readonly IHttpClientFactory _clientFactory;
        public ILogger Logger { get; set; } = new NullLogger();

        #endregion Dependancies

        #region Constructor

        public PostingApiService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        #endregion

        #region Implementation

        public void Init(string siteName, string apiKey)
        {
            site = siteName;
            api = apiKey;
        }

        public async Task<IList<Posting>> GetPostings()
        {
            if (string.IsNullOrEmpty(site))
            {
                Logger.Error("Initialise the API and set siteName before calling this method.");
                return null;
            }

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{URL}{site}");
                var client = _clientFactory.CreateClient();

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<IList<Posting>>(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception e)
            {
                Logger.Error(string.Format("{0}, Error retrieving data from API.", e));
            }

            return null;
        }

        public async Task<PostingResult> Apply(LeverPostingApplyViewModel model)
        {
            if (string.IsNullOrEmpty(site) || string.IsNullOrEmpty(api))
            {
                Logger.Error("Initialise the API and set siteName and apiKey before calling this method.");
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

                var response = await client.PostAsync($"{URL}{site}/{model.PostingId}?key={api}", new StringContent(model.ToJson, Encoding.UTF8, "application/json"));

                return JsonConvert.DeserializeObject<PostingResult>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception e)
            {
                Logger.Error(string.Format("{0}, Error apply for posting id: {1}", e, model.PostingId));
            }

            return null;
        }

        #endregion
    }

    public interface IPostingApiService
    {
        Task<PostingResult> Apply(LeverPostingApplyViewModel model);
        Task<IList<Posting>> GetPostings();
        void Init(string siteName, string apiKey);
    }
}
