using System;
using System.Text.RegularExpressions;

namespace Weapsy.Domain.Sites.Rules
{
    public class SiteRules : ISiteRules
    {
        private readonly ISiteRepository _siteRepository;

        public SiteRules(ISiteRepository siteRepository)
        {
            _siteRepository = siteRepository;
        }

        public bool DoesSiteExist(Guid id)
        {
            var site = _siteRepository.GetById(id);
            return site != null && site.Status != SiteStatus.Deleted;
        }

        public bool IsSiteNameValid(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            var regex = new Regex(@"^[A-Za-z\d\s_-]+$");
            var match = regex.Match(name);
            return match.Success;
        }

        public bool IsSiteIdUnique(Guid id)
        {
            return _siteRepository.GetById(id) == null;
        }

        public bool IsSiteNameUnique(string name)
        {
            var site = _siteRepository.GetByName(name);
            return site == null || site.Status == SiteStatus.Deleted;
        }

        public bool IsSiteUrlValid(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return false;
            var regex = new Regex(@"^[A-Za-z\d_-]+$");
            var match = regex.Match(url);
            return match.Success;
        }

        public bool IsSiteUrlUnique(string url, Guid siteId = new Guid())
        {
            var site = _siteRepository.GetByUrl(url);
            return IsSiteUnique(site, siteId);
        }

        private bool IsSiteUnique(Site site, Guid siteId)
        {
            return site == null
                || site.Status == SiteStatus.Deleted
                || (siteId != Guid.Empty && site.Id == siteId);
        }

        public bool IsPageSetAsHomePage(Guid siteId, Guid pageId)
        {
            var site = _siteRepository.GetById(siteId);

            if (site == null)
                return false;

            return site.HomePageId == pageId;
        }
    }
}