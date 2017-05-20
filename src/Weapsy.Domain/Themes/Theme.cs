using FluentValidation;
using System;
using Weapsy.Framework.Domain;
using Weapsy.Domain.Themes.Commands;
using Weapsy.Domain.Themes.Events;

namespace Weapsy.Domain.Themes
{
    public class Theme : AggregateRoot
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Folder { get; private set; }
        public int SortOrder { get; private set; }
        public ThemeStatus Status { get; private set; }

        public Theme(){}

        private Theme(CreateTheme cmd, IThemeSortOrderGenerator themeSortOrderGenerator)
            : base(cmd.Id)
        {
            Name = cmd.Name;
            Description = cmd.Description;
            Folder = cmd.Folder;
            SortOrder = themeSortOrderGenerator.GenerateNextSortOrder();
            Status = ThemeStatus.Hidden;

            AddEvent(new ThemeCreated
            {
                AggregateRootId = Id,
                Name = Name,
                Description = Description,
                Folder = Folder,
                SortOrder = SortOrder,
                Status = Status
            });
        }

        public static Theme CreateNew(CreateTheme cmd,
            IValidator<CreateTheme> validator,            
            IThemeSortOrderGenerator sortOrderGenerator)
        {
            validator.ValidateCommand(cmd);

            return new Theme(cmd, sortOrderGenerator);
        }

        public void UpdateDetails(UpdateThemeDetails cmd, IValidator<UpdateThemeDetails> validator)
        {
            validator.ValidateCommand(cmd);

            Name = cmd.Name;
            Description = cmd.Description;
            Folder = cmd.Folder;

            AddEvent(new ThemeDetailsUpdated
            {
                AggregateRootId = Id,
                Name = Name,
                Description = Description,
                Folder = Folder
            });
        }

        public void Reorder(int sortOrder)
        {
            if (Status == ThemeStatus.Deleted)
                throw new Exception("Theme is deleted and cannot be reordered.");

            SortOrder = sortOrder;

            AddEvent(new ThemeReordered
            {
                AggregateRootId = Id,
                SortOrder = sortOrder
            });
        }

        public void Activate()
        {
            if (Status == ThemeStatus.Active)
                throw new Exception("Theme already active.");

            Status = ThemeStatus.Active;

            AddEvent(new ThemeActivated
            {
                AggregateRootId = Id
            });
        }

        public void Hide()
        {
            if (Status == ThemeStatus.Hidden)
                throw new Exception("Theme already hidden.");

            if (Status == ThemeStatus.Deleted)
                throw new Exception("Theme is deleted.");

            Status = ThemeStatus.Hidden;

            AddEvent(new ThemeHidden
            {
                AggregateRootId = Id
            });
        }

        public void Delete()
        {
            if (Status == ThemeStatus.Deleted)
                throw new Exception("Theme already deleted.");

            Status = ThemeStatus.Deleted;

            AddEvent(new ThemeDeleted
            {
                AggregateRootId = Id
            });
        }

        public void Restore()
        {
            throw new NotImplementedException();
        }
    }
}
