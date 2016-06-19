#region Copyright

//  Weapsy - http://www.weapsy.org
//  Copyright (c) 2011-2013 Luca Briguglia
//  Licensed under the Weapsy Public License Version 1.0 (WPL-1.0)
//  A copy of this license may be found at http://www.weapsy.org/license

#endregion

using System;

namespace Weapsy.Entities
{
    public abstract class BaseAuditEntity : BaseEntity
    {
        public int CreatedByUserId { get; set; }
        public string CreatedByIpAddress { get; set; }
        public DateTime CreatedOnDate { get; set; }

        public int ModifiedByUserId { get; set; }
        public string ModifiedByIpAddress { get; set; }
        public DateTime ModifiedOnDate { get; set; }
    }
}