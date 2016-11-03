using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Weapsy.Infrastructure.Configuration;

namespace Weapsy.Domain.EventStore.SqlServer
{
    public class EventStoreDbContext : DbContext
    {
        private ConnectionStrings ConnectionStrings { get; set; }

        public EventStoreDbContext(IOptions<ConnectionStrings> settings)
        {
            ConnectionStrings = settings.Value;
        }

        public EventStoreDbContext(DbContextOptions<EventStoreDbContext> options) : base(options) {}

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (ConnectionStrings != null && !string.IsNullOrEmpty(ConnectionStrings.DefaultConnection))
                builder.UseSqlServer(ConnectionStrings.DefaultConnection);
            base.OnConfiguring(builder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<DomainAggregate>()
                .ToTable("DomainAggregate");

            builder.Entity<DomainEvent>()
                .ToTable("DomainEvent")
                .HasKey(x => new { x.AggregateId, x.SequenceNumber });
        }
    }
}
