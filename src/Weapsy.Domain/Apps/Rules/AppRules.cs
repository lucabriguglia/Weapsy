using System;
using System.Text.RegularExpressions;

namespace Weapsy.Domain.Apps.Rules
{
    public class AppRules : IAppRules
    {
        private readonly IAppRepository _appRepository;

        public AppRules(IAppRepository appRepository)
        {
            _appRepository = appRepository;
        }

        public bool DoesAppExist(Guid id)
        {
            var app = _appRepository.GetById(id);
            return app != null && app.Status != AppStatus.Deleted;
        }

        public bool IsAppIdUnique(Guid id)
        {
            return _appRepository.GetById(id) == null;
        }

        public bool IsAppNameUnique(string name, Guid appId = new Guid())
        {
            var app = _appRepository.GetByName(name);
            return IsAppUnique(app, appId);
        }

        public bool IsAppFolderValid(string folder)
        {
            if (string.IsNullOrWhiteSpace(folder)) return false;
            var regex = new Regex(@"^[A-Za-z\.\d_-]+$");
            var match = regex.Match(folder);
            return match.Success;
        }

        public bool IsAppFolderUnique(string folder, Guid appId = new Guid())
        {
            var app = _appRepository.GetByFolder(folder);
            return IsAppUnique(app, appId);
        }

        private bool IsAppUnique(App app, Guid appId)
        {
            return app == null
                || app.Status == AppStatus.Deleted
                || (appId != Guid.Empty && app.Id == appId);
        }
    }
}