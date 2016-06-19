#region Copyright

//  Weapsy - http://www.weapsy.org
//  Copyright (c) 2011-2013 Luca Briguglia
//  Licensed under the Weapsy Public License Version 1.0 (WPL-1.0)
//  A copy of this license may be found at http://www.weapsy.org/license

#endregion

namespace Weapsy.Entities
{
    using System.Collections.Generic;

    public partial class Site : BaseAuditEntity
    {
        public Site()
        {
            LocalizedSites = new HashSet<LocalizedSite>();
            Languages = new HashSet<Language>();
            Pages = new HashSet<Page>();
            Modules = new HashSet<Module>();
            Plugins = new HashSet<Plugin>();
            Widgets = new HashSet<Widget>();
        }

        public string Name { get; set; }
        public int LanguageId { get; set; }
        public bool LanguageSelectionEnabled { get; set; }
        public bool LanguageUrlEnabled { get; set; }
        public int ThemeId { get; set; }
        public string Title { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public bool Closed { get; set; }
        public int PageTemplateId { get; set; }
        public int ModuleTemplateId { get; set; }
        public int HomePageId { get; set; }
        public string Copyright { get; set; }
        public string Logo { get; set; }
        public string Url { get; set; }
        public bool Deleted { get; set; }

        public int RegisterMailTemplateId { get; set; }
        public int RegisterEmailAccountId { get; set; }

        public int PasswordRecoveryMailTemplateId { get; set; }
        public int PasswordRecoveryEmailAccountId { get; set; }

        public int ChangePasswordMailTemplateId { get; set; }
        public int ChangePasswordEmailAccountId { get; set; }

        //public string AccountLayout { get; set; }
        //public string AdminLayout { get; set; }
        //public string AuthLayout { get; set; }
        //public string GroupLayout { get; set; }
        //public string ProfileLayout { get; set; }
        //public string SiteLayout { get; set; }

        public virtual ICollection<LocalizedSite> LocalizedSites { get; set; }
        public virtual ICollection<Language> Languages { get; set; }
        public virtual ICollection<Page> Pages { get; set; }
        public virtual ICollection<MailTemplate> MailTemplates { get; set; }
        public virtual ICollection<Module> Modules { get; set; }
        public virtual ICollection<Plugin> Plugins { get; set; }
        public virtual ICollection<Widget> Widgets { get; set; }

        //public Page HomePage { get; set; }
        //public Theme Theme { get; set; }
        //public PageTemplate PageTemplate { get; set; }
        //public ModuleTemplate ModuleTemplate { get; set; }
        //public Language Language { get; set; }
    }
}