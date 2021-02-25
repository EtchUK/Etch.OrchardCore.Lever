using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.Entities;
using OrchardCore.Settings;

namespace Etch.OrchardCore.Lever.Feeds.Extensions
{
    public static class SiteSettingsExtension
    {
        public static string GetSiteNameValue(this ISite siteSettings)
        {
            return siteSettings?.As<ContentItem>("LeverFeedSettings")?
                    .Get<ContentPart>("LeverFeedSettings")?
                    .Get<TextField>("SiteName")?
                    .Text;
        }

        public static string GetSiteIdValue(this ISite siteSettings)
        {
            return siteSettings?.As<ContentItem>("LeverFeedSettings")?
                .Get<ContentPart>("LeverFeedSettings")?
                .Get<TextField>("SiteId")?
                .Text;
        }

        public static string GetCountryValue(this ISite siteSettings)
        {
            return siteSettings?.As<ContentItem>("LeverFeedSettings")?
                    .Get<ContentPart>("LeverFeedSettings")?
                    .Get<TextField>("Country")?
                    .Text;
        }
    }
}
