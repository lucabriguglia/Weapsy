using System.Collections.Generic;
using FluentValidation;
using Weapsy.Core.Domain;
using Weapsy.Domain.Model.Pages.Commands;
using System;
using Weapsy.Domain.Model.Pages.Events;

namespace Weapsy.Domain.Model.Pages.Handlers
{
    public class UpdatePageDetailsHandler : ICommandHandler<UpdatePageDetails>
    {
        private readonly IPageRepository _pageRepository;
        private readonly IValidator<UpdatePageDetails> _validator;

        public UpdatePageDetailsHandler(IPageRepository pageRepository,
            IValidator<UpdatePageDetails> validator)
        {
            _pageRepository = pageRepository;
            _validator = validator;
        }

        public ICollection<IEvent> Handle(UpdatePageDetails command)
        {
            var page = _pageRepository.GetById(command.SiteId, command.Id);

            if (page == null)
                throw new Exception("Page not found");

            page.UpdateDetails(command, _validator);

            _pageRepository.Update(page);

            return page.Events;

            return new List<IEvent>
            {
                new PageDetailsUpdated
                {
                    SiteId = page.SiteId,
                    AggregateRootId = page.Id,
                    Name = page.Name,
                    Url = page.Url,
                    Title = page.Title,
                    MetaDescription = page.MetaDescription,
                    MetaKeywords = page.MetaKeywords,
                    PageLocalisations = page.PageLocalisations
                }
            };
        }
    }
}
