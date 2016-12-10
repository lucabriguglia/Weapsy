using System;

namespace Weapsy.Mvc.Apps
{
    public class AppDescriptor
    {
        public AppDescriptor(string folder)
        {
            Folder = folder;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Folder { get; set; }
    }
}
