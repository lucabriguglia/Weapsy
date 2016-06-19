#region Copyright

//  Weapsy - http://www.weapsy.org
//  Copyright (c) 2011-2013 Luca Briguglia
//  Licensed under the Weapsy Public License Version 1.0 (WPL-1.0)
//  A copy of this license may be found at http://www.weapsy.org/license

#endregion

namespace Weapsy.Entities
{
    using System;

    public class Friendship : BaseEntity
    {
        public int CreatedByUserId { get; set; }
        public string CreatedByIpAddress { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public int ReceivedByUserId { get; set; }
        public bool Accepted { get; set; }
        public string AcceptedByIpAddress { get; set; }
        public DateTime? AcceptedOnDate { get; set; }
    }
}
