#region Copyright

//  Weapsy - http://www.weapsy.org
//  Copyright (c) 2011-2013 Luca Briguglia
//  Licensed under the Weapsy Public License Version 1.0 (WPL-1.0)
//  A copy of this license may be found at http://www.weapsy.org/license

#endregion

using System.Collections.Generic;

namespace Weapsy.Entities
{
    public class ModuleDefinition : BaseAuditEntity
    {
        public ModuleDefinition()
        {
            Modules = new HashSet<Module>();
        }
    
        public int AppId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string AdminController { get; set; }
        public string AdminAction { get; set; }
        public bool Deleted { get; set; }

        //public virtual App App { get; set; }

        public virtual ICollection<Module> Modules { get; set; }
    }
}