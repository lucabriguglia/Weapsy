using System;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Weapsy.Domain.Data.SqlServer
{
    public class DbContextFactory : IDbContextFactory<WeapsyDbContext>
    {
        public WeapsyDbContext Create(DbContextFactoryOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
