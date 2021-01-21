using Etch.OrchardCore.Lever.ViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Etch.OrchardCore.Lever.Extensions
{
    public static class LeverJsonExtensions
    {
        public static string ToJson(this LeverPostingApplyViewModel model)
        {
            var json = new Dictionary<string, object> {
                { "name", !string.IsNullOrEmpty(model.Name) ? model.Name : string.Empty },
                { "email", !string.IsNullOrEmpty(model.Email) ? model.Email : string.Empty },
                { "comments", !string.IsNullOrEmpty(model.Comments) ? model.Comments : string.Empty },
                { "org", !string.IsNullOrEmpty(model.Org) ? model.Org : string.Empty },
                { "phone", !string.IsNullOrEmpty(model.Phone) ? model.Phone : string.Empty },
            };

            if (model.Urls != null && model.Urls.Any())
            {
                json.Add("urls", model.Urls);
            }

            if (model.Consent != null && model.Consent.Any())
            {
                json.Add("consent", model.Consent);
            }

            if (model.CustomQuestions != null && model.CustomQuestions.Fields.Any())
            {
                json.Add("cards", new Dictionary<string, Dictionary<string, string>> {
                    {
                        model.CustomQuestions.Id,
                        model.CustomQuestions.Fields
                    }
                });
            }

            return JsonConvert.SerializeObject(json);
        }
    }
}
