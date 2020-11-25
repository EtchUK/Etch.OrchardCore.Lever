using System;
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

        private string apiKey = null;

        private const string URL1 = "https://run.mocky.io/v3/877ecca6-4e8b-4242-810d-ed5cfa10cb99";
        //private const string URL1 = "https://run.mocky.io/v3/24adbdeb-c021-444e-8146-7a84e092855f";

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

        public async Task<PostingData> GetPostings()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, URL1);
                var client = _clientFactory.CreateClient();

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<PostingData>(await response.Content.ReadAsStringAsync());
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
        Task<PostingData> GetPostings();
    }
}
