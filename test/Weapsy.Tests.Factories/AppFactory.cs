using System;
using Weapsy.Domain.Apps;
using Weapsy.Domain.Apps.Commands;

namespace Weapsy.Tests.Factories
{
    public static class AppFactory
    {
        public static App CreateApp()
        {
            return CreateApp(Guid.NewGuid(), "Name", "Description", "Folder");
        }

        public static App CreateApp(Guid id, string name, string description, string folder)
        {
            var command = new CreateApp
            {
                Id = id,
                Name = name,
                Description = description,
                Folder = folder
            };

            return new App(command);
        }
    }
}
