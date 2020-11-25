using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Etch.OrchardCore.Lever.Api.Models.Dto;
using Newtonsoft.Json;

namespace Etch.OrchardCore.Lever.Api.Services
{
    public class PostingApiService : IPostingApiService
    {
        #region Constants

        private const string URL = "https://api.lever.co/v0/postings/";

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

        public async Task<IList<Posting>> GetPostings(string site)
        {
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

        #endregion
    }

    public interface IPostingApiService
    {
        Task<IList<Posting>> GetPostings(string site);
    }
}
