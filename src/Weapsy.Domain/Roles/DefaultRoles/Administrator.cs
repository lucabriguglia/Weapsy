using System;

namespace Weapsy.Domain.Roles.DefaultRoles
{
    public static class Administrator
    {
        public static Guid Id { get; } = new Guid("b525bee0-edac-441f-87ca-d5e34a356c4a");
        public static string Name { get; } = "Administrator";
    }
}
