using Etch.OrchardCore.Lever.ViewModels;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Etch.OrchardCore.Lever.Extensions
{
    public static class LeverFormDataExtensions
    {
        #region Public Methods

        public static MultipartFormDataContent ToFormData(this LeverPostingApplyViewModel model)
        {
            var httpContent = new MultipartFormDataContent
            {
                { new StringContent(!string.IsNullOrEmpty(model.Comments) ? model.Comments : string.Empty), FormatKeyForLever("comments") },
                { new StringContent(!string.IsNullOrEmpty(model.Email) ? model.Email : string.Empty), FormatKeyForLever("email") },
                { new StringContent(!string.IsNullOrEmpty(model.Name) ? model.Name : string.Empty), FormatKeyForLever("name") },
                { new StringContent(!string.IsNullOrEmpty(model.Org) ? model.Org : string.Empty), FormatKeyForLever("org") },
                { new StringContent(!string.IsNullOrEmpty(model.Phone) ? model.Phone : string.Empty), FormatKeyForLever("phone") }
            };

            SetUrlsToFromData(model, httpContent);
            SetConsentToFromData(model, httpContent);
            SetCardsToFromData(model, httpContent);
            SetResumeToFromData(model, httpContent);

            return httpContent;
        }

        #endregion

        #region Private Methods

        // Double qoutes are required for keys as the Lever API doesn't accept the key without double qoutes.
        // Solution from: https://stackoverflow.com/a/21874413
        private static string FormatKeyForLever(string value)
        {
            return string.Format(@"""{0}""", value);
        }

        private static void SetConsentToFromData(LeverPostingApplyViewModel model, MultipartFormDataContent httpContent)
        {
            if (model.Consent == null || !model.Consent.Any())
            {
                return;
            }

            foreach (var item in model.Consent.Where(x => x.Value != null))
            {
                httpContent.Add(new StringContent(item.Value.ToLower() == "on" ? "true" : "false"), string.Format(FormatKeyForLever("consent[{0}]"), item.Key));
            }
        }

        private static void SetCardsToFromData(LeverPostingApplyViewModel model, MultipartFormDataContent httpContent)
        {
            if (model.CustomQuestions == null || !model.CustomQuestions.Fields.Any())
            {
                return;
            }

            foreach (var item in model.CustomQuestions.Fields.Where(x => x.Value != null))
            {
                httpContent.Add(new StringContent(item.Value), string.Format(FormatKeyForLever("cards[{0}][{1}]"), model.CustomQuestions.Id, item.Key));
            }
        }

        private static void SetResumeToFromData(LeverPostingApplyViewModel model, MultipartFormDataContent httpContent)
        {
            if (model.Resume == null)
            {
                return;
            }

            var fileContent = new StreamContent(model.Resume.OpenReadStream())
            {
                Headers =
                {
                    ContentLength = model.Resume.Length,
                    ContentType = new MediaTypeHeaderValue(model.Resume.ContentType)
                }
            };

            httpContent.Add(fileContent, FormatKeyForLever("resume"), model.Resume.FileName);
        }

        private static void SetUrlsToFromData(LeverPostingApplyViewModel model, MultipartFormDataContent httpContent)
        {
            if (model.Urls == null || !model.Urls.Any())
            {
                return;
            }

            foreach (var item in model.Urls.Where(x => x.Value != null))
            {
                httpContent.Add(new StringContent(item.Value), FormatKeyForLever(string.Format("urls[{0}]", item.Key)));
            }
        }

        #endregion
    }
}
