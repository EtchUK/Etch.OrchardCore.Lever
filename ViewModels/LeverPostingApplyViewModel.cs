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
        public Dictionary<string, Dictionary<string, string>> Cards { get; set; }
        public CustomQuestions CustomQuestions { get; set; } = new CustomQuestions();
        public string Comments { get; set; }
        public Dictionary<string, string> Consent { get; set; }
        public string Email { get; set; }
        public IFormFile Resume { get; set; }
        [Required]
        public string Name { get; set; }
        public string Org { get; set; }
        [Required]
        public string Phone { get; set; }
        public string PostingId { get; set; }
        public Dictionary<string, string> Urls { get; set; }

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

        public MultipartFormDataContent ToDataFrom()
        {
            var httpContent = new MultipartFormDataContent
            {
                // Double qoutes are required for keys as the Lever API doesn't accept the key without double qoutes.
                // Solution from: https://stackoverflow.com/a/21874413
                { new StringContent(!string.IsNullOrEmpty(Comments) ? Comments : string.Empty), @"""comments""" },
                { new StringContent(!string.IsNullOrEmpty(Email) ? Email : string.Empty), @"""email""" },
                { new StringContent(!string.IsNullOrEmpty(Name) ? Name : string.Empty), @"""name""" },
                { new StringContent(!string.IsNullOrEmpty(Org) ? Org : string.Empty), @"""org""" },
                { new StringContent(!string.IsNullOrEmpty(Phone) ? Phone : string.Empty), @"""phone""" }
            };

            if (Urls != null && Urls.Any())
            {
                foreach (var item in Urls)
                {
                    if (item.Value == null)
                    {
                        continue;
                    }

                    httpContent.Add(new StringContent(item.Value), string.Format(@"""urls[{0}]""", item.Key));
                }
            }

            if (Consent != null && Consent.Any())
            {
                foreach (var item in Consent)
                {
                    if (item.Value == null)
                    {
                        continue;
                    }

                    httpContent.Add(new StringContent(item.Value.ToLower() == "on" ? "true" : "false"), string.Format(@"""consent[{0}]""", item.Key));
                }
            }

            if (CustomQuestions != null && CustomQuestions.Fields.Any())
            {
                foreach (var item in CustomQuestions.Fields)
                {
                    if (item.Value == null)
                    {
                        continue;
                    }

                    httpContent.Add(new StringContent(item.Value), string.Format(@"""cards[{0}][{1}]""", CustomQuestions.Id, item.Key));
                }
            }

            if (Resume != null)
            {
                var fileContent = new StreamContent(Resume.OpenReadStream())
                {
                    Headers =
                    {
                        ContentLength = Resume.Length,
                        ContentType = new MediaTypeHeaderValue(Resume.ContentType)
                    }
                };

                httpContent.Add(fileContent, @"""resume""", Resume.FileName);
            }

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

        private string GetCustomQuestionId(string field)
        {
            var items = new Regex(@"\[(.*?)\]").Match(field);

            if (items.Length == 0)
            {
                return null;
            }

            return Regex.Replace(items.Value, @"[\[\]]", "");
        }
    }

    public class CustomQuestions
    {
        public string Id { get; set; }
        public Dictionary<string, string> Fields { get; set; } = new Dictionary<string, string>();
    }
}
