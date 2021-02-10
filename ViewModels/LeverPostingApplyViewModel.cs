using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Etch.OrchardCore.Lever.ViewModels
{
    public class LeverPostingApplyViewModel
    {
        #region Constants

        public Dictionary<string, Dictionary<string, string>> Cards { get; set; }
        public CustomQuestions CustomQuestions { get; set; } = new CustomQuestions();
        public CustomQuestions CustomSurveyQuestions { get; set; } = new CustomQuestions();
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

        public void UpdateSurveysResponses(IFormCollection form)
        {
            var count = 0;
            foreach (var item in form.Keys)
            {
                if (!item.ToLower().StartsWith("surveysresponses"))
                {
                    continue;
                }

                if (CustomSurveyQuestions.Id == null)
                {
                    CustomSurveyQuestions.Id = GetCustomQuestionId(item);
                }

                if (!string.IsNullOrEmpty(form[item]))
                {
                    CustomSurveyQuestions.Fields.Add($"field{count}", form[item]);
                    count++;
                    continue;
                }

                CustomSurveyQuestions.Fields.Add($"field{count}", "");
                count++;
            }
        }

        #endregion

        #region Private

        private string GetCustomQuestionId(string field)
        {
            var items = new Regex(@"\[(.*?)\]").Match(field);

            if (items.Length == 0)
            {
                return null;
            }

            return Regex.Replace(items.Value, @"[\[\]]", "");
        }

        #endregion
    }

    public class CustomQuestions
    {
        public string Id { get; set; }
        public Dictionary<string, string> Fields { get; set; } = new Dictionary<string, string>();
    }
}
