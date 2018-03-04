using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Weapsy.Apps.Text.Domain;
using TextModuleDbEntity = Weapsy.Apps.Text.Data.Entities.TextModule;
using TextVersionDbEntity = Weapsy.Apps.Text.Data.Entities.TextVersion;
using TextLocalisationDbEntity = Weapsy.Apps.Text.Data.Entities.TextLocalisation;

namespace Weapsy.Apps.Text.Data.Domain
{
    public class TextModuleRepository : ITextModuleRepository
    {
        private readonly IContextFactory _dbContextFactory;
        private readonly IMapper _mapper;

        public TextModuleRepository(IContextFactory dbContextFactory, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _mapper = mapper;
        }

        public TextModule GetById(Guid id)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.TextModules.FirstOrDefault(x => x.Id == id);
                LoadTextVersions(context, dbEntity);
                return dbEntity != null ? _mapper.Map<TextModule>(dbEntity) : null;                
            }
        }

        public TextModule GetByModuleId(Guid moduleId)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.TextModules.FirstOrDefault(x => x.ModuleId == moduleId);
                LoadTextVersions(context, dbEntity);
                return dbEntity != null ? _mapper.Map<TextModule>(dbEntity) : null;
            }
        }

        public async Task CreateAsync(TextModule text)
        {
            using (var context = _dbContextFactory.Create())
            {
                var textDbEntity = _mapper.Map<TextModuleDbEntity>(text);
                context.TextModules.Add(textDbEntity);
                await context.SaveChangesAsync();
            }
        }

        public void Update(TextModule textModule)
        {
            using (var context = _dbContextFactory.Create())
            {
                var textDbEntity = context.TextModules.FirstOrDefault(x => x.Id == textModule.Id);

                textDbEntity.Status = textModule.Status;

                UpdateTextVersions(context, textModule.TextVersions);

                context.SaveChanges();
            }
        }

        private void UpdateTextVersions(TextDbContext context, IList<TextVersion> textVersions)
        {
            foreach (var textVersion in textVersions)
            {
                var textVersionDbEntity = context.TextVersions.FirstOrDefault(x => x.Id == textVersion.Id);

                if (textVersionDbEntity == null)
                {
                    context.TextVersions.Add(_mapper.Map<TextVersionDbEntity>(textVersion));
                }
                else
                {
                    textVersionDbEntity.Description = textVersion.Description;
                    textVersionDbEntity.Content = textVersion.Content;
                    textVersionDbEntity.Status = textVersion.Status;

                    UpdateTextVersionLocalisations(context, textVersion.TextLocalisations);
                }
            }
        }

        private void UpdateTextVersionLocalisations(TextDbContext context, ICollection<TextLocalisation> textVersionLocalisations)
        {
            foreach (var textVersionLocalisation in textVersionLocalisations)
            {
                var textVersionLocalisationDbEntity = context.TextLocalisations
                    .FirstOrDefault(x =>
                        x.TextVersionId == textVersionLocalisation.TextVersionId &&
                        x.LanguageId == textVersionLocalisation.LanguageId);

                if (textVersionLocalisationDbEntity == null)
                {
                    context.TextLocalisations.Add(_mapper.Map<TextLocalisationDbEntity>(textVersionLocalisation));
                }
                else
                {
                    textVersionLocalisationDbEntity.Content = textVersionLocalisation.Content;
                }
            }
        }

        private void LoadTextVersions(TextDbContext context, TextModuleDbEntity textDbEntity)
        {
            if (textDbEntity != null)
            {
                textDbEntity.TextVersions = context.TextVersions
                    .Include(y => y.TextLocalisations)
                    .Where(x => x.TextModuleId == textDbEntity.Id && x.Status != TextVersionStatus.Deleted)
                    .ToList();
            }
        }
    }
}

