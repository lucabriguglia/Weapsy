using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Weapsy.Domain.Pages.Rules
{
    public class PageRules : IPageRules
    {
        private readonly IPageRepository _pageRepository;

        public PageRules(IPageRepository pageRepository)
        {
            _pageRepository = pageRepository;
        }

        public bool DoesPageExist(Guid id)
        {
            var page = _pageRepository.GetById(id);
            return page != null && page.Status != PageStatus.Deleted;
        }

        public bool DoesPageExist(Guid siteId, Guid pageId)
        {
            var page = _pageRepository.GetById(siteId, pageId);
            return page != null && page.Status != PageStatus.Deleted;
        }

        public bool IsPageIdUnique(Guid id)
        {
            return _pageRepository.GetById(id) == null;
        }

        public bool IsPageNameValid(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            var regex = new Regex(@"^[A-Za-z\d\s_-]+$");
            var match = regex.Match(name);
            return match.Success;
        }

        public bool IsPageNameUnique(Guid siteId, string name, Guid pageId = new Guid())
        {
            var pageIdByName = _pageRepository.GetPageIdByName(siteId, name);
            return pageIdByName == Guid.Empty || (pageId != Guid.Empty && pageIdByName == pageId);
        }

        public bool IsPageUrlValid(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return false;
            var regex = new Regex(@"^[A-Za-z/\d_-]+$");
            var match = regex.Match(url);
            return match.Success;
        }

        public bool IsPageUrlReserved(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return false;
            return ReservedUrls().FirstOrDefault(x => x == url || url.StartsWith(string.Format("{0}/", x))) != null;
        }

        private string[] ReservedUrls()
        {
            return new string[] 
            {
                "account",
                "app",
                "admin",
                "auth",
                "error",
                "group",
                "module",
                "profile",
                "text",
                "user"
            };
        }

        public bool IsSlugUnique(Guid siteId, string slug, Guid pageId = new Guid())
        {
            if (string.IsNullOrWhiteSpace(slug))
                return true;
            var pageIdBySlug = _pageRepository.GetPageIdBySlug(siteId, slug);
            var pageIdByLocalisedSlug = _pageRepository.GetPageIdByLocalisedSlug(siteId, slug);
            return (pageIdBySlug == Guid.Empty || (pageId != Guid.Empty && pageIdBySlug == pageId))
                && (pageIdByLocalisedSlug == Guid.Empty || (pageId != Guid.Empty && pageIdByLocalisedSlug == pageId));
        }

        private bool IsPageUnique(Page page, Guid pageId)
        {
            return page == null 
                || page.Status == PageStatus.Deleted 
                || (pageId != Guid.Empty && page.Id == pageId);
        }
    }
}