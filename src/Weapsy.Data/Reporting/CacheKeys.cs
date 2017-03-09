namespace Weapsy.Data.Reporting
{
    public class CacheKeys
    {
        public const string LanguagesCacheKey = "Languages|SiteId:{0}";
        public const string MenuCacheKey = "Menu|SiteId:{0}|Name:{1}|LanguageId:{2}";
        public const string ModuleTypesCacheKey = "ModuleTypes";
        public const string PageInfoCacheKey = "PageInfo|SiteId:{0}|PageId:{1}|LanguageId:{2}";
        public const string SiteInfoCacheKey = "SiteInfo|SiteName:{0}|LanguageId:{1}";
        public const string ThemesCacheKey = "Themes";
    }
}
