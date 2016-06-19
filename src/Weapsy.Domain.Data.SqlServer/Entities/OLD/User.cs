#region Copyright

//  Weapsy - http://www.weapsy.org
//  Copyright (c) 2011-2013 Luca Briguglia
//  Licensed under the Weapsy Public License Version 1.0 (WPL-1.0)
//  A copy of this license may be found at http://www.weapsy.org/license

#endregion

using System;

namespace Weapsy.Entities
{
    public partial class User : BaseAuditEntity
    {
        //public User()
        //{
        //}
    
        public string MembershipName { get; set; }
        public string MembershipEmail { get; set; }
        public DateTime MembershipDate { get; set; }

        public string DisplayName { get; set; }
        public string ProfileName { get; set; }
        public bool Deleted { get; set; }

        //public bool IsSuperAdmin { get; set; }
        //public bool IsAdmin { get; set; }

        //public string FirstName { get; set; }
        //public string MiddleName { get; set; }
        //public string LastName { get; set; }
        //public Nullable<byte> Gender { get; set; }
        //public string City { get; set; }
        //public Nullable<byte> BirthdayDay { get; set; }
        //public Nullable<byte> BirthdayMonth { get; set; }
        //public Nullable<byte> BirthdayYear { get; set; }
        //public int ProfileViews { get; set; }
        //public string ProfileImage { get; set; }
        //public byte MessengerAuthorized { get; set; }
        //public byte NameAuthorized { get; set; }
        //public byte CityAuthorized { get; set; }
        //public byte BirthdayAuthorized { get; set; }
        //public byte GenderAuthorized { get; set; }
        //public bool MessengerNotificationEmail { get; set; }
        //public bool FriendshipNotificationEmail { get; set; }
        //public int ThemeId { get; set; }
    }
}
