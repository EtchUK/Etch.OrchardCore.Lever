using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Etch.OrchardCore.Lever.ViewModels
{
    public class LeverPostingApplyViewModel
    {
        #region Constants

        public Dictionary<string, Dictionary<string, string>> Cards { get; set; }
        public CustomQuestions CustomQuestions { get; set; } = new CustomQuestions();
        public string Comments { get; set; }
        public Dictionary<string, string> Consent { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        public string Org { get; set; }
        [Required]
        public string Phone { get; set; }
        public string PostingId { get; set; }
        public IFormFile Resume { get; set; }
        public Dictionary<string, string> Urls { get; set; }

        #endregion

        #region Implementation

        public string ToJson
        {
            get
            {
                var json = new Dictionary<string, object> {
                    { "name", !string.IsNullOrEmpty(Name) ? Name : string.Empty },
                    { "email", !string.IsNullOrEmpty(Email) ? Email : string.Empty },
                    { "comments", !string.IsNullOrEmpty(Comments) ? Comments : string.Empty },
                    { "org", !string.IsNullOrEmpty(Org) ? Org : string.Empty },
                    { "phone", !string.IsNullOrEmpty(Phone) ? Phone : string.Empty },
                };

                if (Urls != null && Urls.Any())
                {
                    json.Add("urls", Urls);
                }

                if (Consent != null && Consent.Any())
                {
                    json.Add("consent", Consent);
                }

                if (CustomQuestions != null && CustomQuestions.Fields.Any())
                {
                    json.Add("cards", new Dictionary<string, Dictionary<string, string>> {

                        {
                            CustomQuestions.Id,
                            CustomQuestions.Fields
                        }
                    });
                }

                return JsonConvert.SerializeObject(json);
            }
        }

        public MultipartFormDataContent ToFromData()
        {
            var httpContent = new MultipartFormDataContent
            {
                { new StringContent(!string.IsNullOrEmpty(Comments) ? Comments : string.Empty), FormatKeyForLever("comments") },
                { new StringContent(!string.IsNullOrEmpty(Email) ? Email : string.Empty), FormatKeyForLever("email") },
                { new StringContent(!string.IsNullOrEmpty(Name) ? Name : string.Empty), FormatKeyForLever("name") },
                { new StringContent(!string.IsNullOrEmpty(Org) ? Org : string.Empty), FormatKeyForLever("org") },
                { new StringContent(!string.IsNullOrEmpty(Phone) ? Phone : string.Empty), FormatKeyForLever("phone") }
            };

            SetUrlsToFromData(httpContent);
            SetConsentToFromData(httpContent);
            SetCardsToFromData(httpContent);
            SetResumeToFromData(httpContent);

            return httpContent;
        }

        public void UpdateCards(IFormCollection form)
        {
            var count = 0;
            foreach (var item in form.Keys)
            {
                if (!item.ToLower().StartsWith("cards"))
                {
                    continue;
                }

                if (CustomQuestions.Id == null)
                {
                    CustomQuestions.Id = GetCustomQuestionId(item);
                }

                if (!string.IsNullOrEmpty(form[item]))
                {
                    CustomQuestions.Fields.Add($"field{count}", form[item]);
                    count++;
                    continue;
                }

                CustomQuestions.Fields.Add($"field{count}", "");
                count++;
            }
        }

        #endregion

        #region Private

        private void SetUrlsToFromData(MultipartFormDataContent httpContent)
        {
            if (Urls == null || !Urls.Any())
            {
                return;
            }

            foreach (var item in Urls.Where(x => x.Value != null))
            {
                httpContent.Add(new StringContent(item.Value), FormatKeyForLever(string.Format("urls[{0}]", item.Key)));
            }
        }

        private void SetConsentToFromData(MultipartFormDataContent httpContent)
        {
            if (Consent == null || !Consent.Any())
            {
                return;
            }

            foreach (var item in Consent.Where(x => x.Value != null))
            {
                httpContent.Add(new StringContent(item.Value.ToLower() == "on" ? "true" : "false"), string.Format(FormatKeyForLever("consent[{0}]"), item.Key));
            }
        }

        private void SetCardsToFromData(MultipartFormDataContent httpContent)
        {
            if (CustomQuestions == null || !CustomQuestions.Fields.Any())
            {
                return;
            }

            foreach (var item in CustomQuestions.Fields.Where(x => x.Value != null))
            {
                httpContent.Add(new StringContent(item.Value), string.Format(FormatKeyForLever("cards[{0}][{1}]"), CustomQuestions.Id, item.Key));
            }
        }

        private void SetResumeToFromData(MultipartFormDataContent httpContent)
        {
            if (Resume == null)
            {
                return;
            }

            var fileContent = new StreamContent(Resume.OpenReadStream())
            {
                Headers =
                {
                    ContentLength = Resume.Length,
                    ContentType = new MediaTypeHeaderValue(Resume.ContentType)
                }
            };

            httpContent.Add(fileContent, FormatKeyForLever("resume"), Resume.FileName);
        }

        private string GetCustomQuestionId(string field)
        {
            var items = new Regex(@"\[(.*?)\]").Match(field);

            if (items.Length == 0)
            {
                return null;
            }

            return Regex.Replace(items.Value, @"[\[\]]", "");
        }

        private string FormatKeyForLever(string value)
        {
            // Double qoutes are required for keys as the Lever API doesn't accept the key without double qoutes.
            // Solution from: https://stackoverflow.com/a/21874413

            return string.Format(@"""{0}""", value);
        }

        #endregion
    }

    public class CustomQuestions
    {
        public string Id { get; set; }
        public Dictionary<string, string> Fields { get; set; } = new Dictionary<string, string>();
    }
}
