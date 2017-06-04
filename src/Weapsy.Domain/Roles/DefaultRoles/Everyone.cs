using System;

namespace Weapsy.Domain.Roles.DefaultRoles
{
    public static class Everyone
    {
        public static Guid Id { get; } = new Guid("eb8ff2ad-6f83-4c2a-a50f-e11ba247026e");
        public static string Name { get; } = "Everyone";
    }
}
