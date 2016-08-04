using System;

namespace Weapsy.Mvc.Context
{
    public class ContextInfo
    {
        public SiteInfo Site { get; set; }
        public PageInfo Page { get; set; }
        public UserInfo User { get; set; }
        public ThemeInfo Theme { get; set; }
        public LanguageInfo Language { get; set; }
    }

    public class SiteInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class PageInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class UserInfo
    {
        public Guid Id { get; set; }
    }

    public class ThemeInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Folder { get; set; }
    }

    public class LanguageInfo
    {
        public Guid Id { get; set; }
        public string Culture { get; set; }
    }
}
