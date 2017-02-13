using FluentValidation;
using System.Collections.Generic;
using Weapsy.Domain.Apps;
using Weapsy.Domain.ModuleTypes;
using Weapsy.Domain.ModuleTypes.Commands;
using Weapsy.Domain.Apps.Commands;

namespace Weapsy.Services.Installation
{
    public class AppInstallationService : IAppInstallationService
    {
        private readonly IAppRepository _appRepository;
        private readonly IValidator<CreateApp> _createAppValidator;
        private readonly IModuleTypeRepository _moduleTypeRepository;
        private readonly IValidator<CreateModuleType> _createModuleTypeValidator;

        public AppInstallationService(IAppRepository appRepository,
            IValidator<CreateApp> createAppValidator,
            IModuleTypeRepository moduleTypeRepository,
            IValidator<CreateModuleType> createModuleTypeValidator)
        {
            _appRepository = appRepository;
            _createAppValidator = createAppValidator;
            _moduleTypeRepository = moduleTypeRepository;
            _createModuleTypeValidator = createModuleTypeValidator;
        }

        public void EnsureAppInstalled(CreateApp createApp, IEnumerable<CreateModuleType> createModuleTypes)
        {
            if (_appRepository.GetByName(createApp.Name) != null)
                return;

            var app = App.CreateNew(createApp, _createAppValidator);
            _appRepository.Create(app);

            foreach (var createModuleType in createModuleTypes)
            {
                var moduleType = ModuleType.CreateNew(createModuleType, _createModuleTypeValidator);
                _moduleTypeRepository.Create(moduleType);
            }
        }
    }
}
