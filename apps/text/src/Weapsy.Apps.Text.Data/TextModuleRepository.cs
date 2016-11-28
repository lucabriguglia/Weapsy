using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Weapsy.Apps.Text.Domain;
using TextModuleDbEntity = Weapsy.Apps.Text.Data.Entities.TextModule;
using TextVersionDbEntity = Weapsy.Apps.Text.Data.Entities.TextVersion;
using TextLocalisationDbEntity = Weapsy.Apps.Text.Data.Entities.TextLocalisation;

namespace Weapsy.Apps.Text.Data
{
    public class TextModuleRepository : ITextModuleRepository
    {
        private readonly TextModuleDbContext _context;
        private readonly DbSet<TextModuleDbEntity> _texts;
        private readonly DbSet<TextVersionDbEntity> _textVersions;
        private readonly DbSet<TextLocalisationDbEntity> _textVersionLocalisations;
        private readonly IMapper _mapper;

        public TextModuleRepository(TextModuleDbContext context, IMapper mapper)
        {
            _context = context;
            _texts = context.Set<TextModuleDbEntity>();
            _textVersions = context.Set<TextVersionDbEntity>();
            _textVersionLocalisations = context.Set<TextLocalisationDbEntity>();
            _mapper = mapper;
        }

        public TextModule GetById(Guid id)
        {
            var dbEntity = _texts.FirstOrDefault(x => x.Id == id);
            LoadTextVersions(dbEntity);
            return dbEntity != null ? _mapper.Map<TextModule>(dbEntity) : null;
        }

        public TextModule GetByModuleId(Guid moduleId)
        {
            var dbEntity = _texts.FirstOrDefault(x => x.ModuleId == moduleId);
            LoadTextVersions(dbEntity);
            return dbEntity != null ? _mapper.Map<TextModule>(dbEntity) : null;
        }

        public void Create(TextModule text)
        {
            var textDbEntity = _mapper.Map<TextModuleDbEntity>(text);
            _texts.Add(textDbEntity);
            _context.SaveChanges();
        }

        public void Update(TextModule textModule)
        {
            var textDbEntity = _texts.FirstOrDefault(x => x.Id == textModule.Id);

            textDbEntity.Status = textModule.Status;

            UpdateTextVersions(textModule.TextVersions);

            _context.SaveChanges();
        }

        private void UpdateTextVersions(IList<TextVersion> textVersions)
        {
            foreach (var textVersion in textVersions)
            {
                var textVersionDbEntity = _textVersions.FirstOrDefault(x => x.Id == textVersion.Id);

                if (textVersionDbEntity == null)
                {
                    _textVersions.Add(_mapper.Map<TextVersionDbEntity>(textVersion));
                }
                else
                {
                    textVersionDbEntity.Description = textVersion.Description;
                    textVersionDbEntity.Content = textVersion.Content;
                    textVersionDbEntity.Status = textVersion.Status;

                    UpdateTextVersionLocalisations(textVersion.TextLocalisations);
                }
            }
        }

        private void UpdateTextVersionLocalisations(ICollection<TextLocalisation> textVersionLocalisations)
        {
            foreach (var textVersionLocalisation in textVersionLocalisations)
            {
                var textVersionLocalisationDbEntity = _textVersionLocalisations
                    .FirstOrDefault(x =>
                        x.TextVersionId == textVersionLocalisation.TextVersionId &&
                        x.LanguageId == textVersionLocalisation.LanguageId);

                if (textVersionLocalisationDbEntity == null)
                {
                    _textVersionLocalisations.Add(_mapper.Map<TextLocalisationDbEntity>(textVersionLocalisation));
                }
                else
                {
                    textVersionLocalisationDbEntity.Content = textVersionLocalisation.Content;
                }
            }
        }

        private void LoadTextVersions(TextModuleDbEntity textDbEntity)
        {
            if (textDbEntity != null)
            {
                textDbEntity.TextVersions = _textVersions
                    .Include(y => y.TextLocalisations)
                    .Where(x => x.TextModuleId == textDbEntity.Id && x.Status != TextVersionStatus.Deleted)
                    .ToList();
            }
        }
    }
}
