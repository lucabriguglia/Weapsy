using System;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Weapsy.Domain.Sites;
using Weapsy.Domain.Sites.Commands;

namespace Weapsy.Tests.Factories
{
    public static class SiteFactory
    {
        public static Site Site()
        {
            return Site(Guid.NewGuid(), "My Site");
        }

        public static Site Site(Guid id, string name)
        {
            var command = new CreateSite
            {
                Id = id,
                Name = name
            };

            var validatorMock = new Mock<IValidator<CreateSite>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            return Domain.Sites.Site.CreateNew(command, validatorMock.Object);
        }
    }
}
