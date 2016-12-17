using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.AspNetCore.Hosting;

namespace Weapsy.Mvc.Apps
{
    public class AppLoader
    {
        private static AppLoader _instance;
        private static IHostingEnvironment _hostingEnvironment;
        private readonly IList<string> _loadedAssemblies = new List<string>();
        public IList<AppDescriptor> AppDescriptors = new List<AppDescriptor>();
        public IList<Assembly> AppAssemblies = new List<Assembly>();

        public static AppLoader Instance(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            return _instance ?? (_instance = new AppLoader());
        }

        private AppLoader()
        {
            var rootFolder = new DirectoryInfo(_hostingEnvironment.ContentRootPath);
            var appsRootFolder = new DirectoryInfo(Path.Combine(_hostingEnvironment.ContentRootPath, "Apps"));

            foreach (var file in rootFolder.GetFiles("*.dll", SearchOption.TopDirectoryOnly))
            {
                if (_loadedAssemblies.FirstOrDefault(x => x == file.Name) == null)
                {
                    _loadedAssemblies.Add(file.Name);
                }
            }

            foreach (var appFolder in appsRootFolder.GetDirectories())
            {
                AppDescriptors.Add(new AppDescriptor(appFolder.Name));

                foreach (var file in appFolder.GetFiles("*.dll", SearchOption.TopDirectoryOnly))
                {
                    if (_loadedAssemblies.FirstOrDefault(x => x == file.Name) != null)
                        continue;

                    _loadedAssemblies.Add(file.Name);

                    try
                    {
                        var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(file.FullName);
                        AppAssemblies.Add(assembly);
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }
        }
    }
}
