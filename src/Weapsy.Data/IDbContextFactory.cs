namespace Weapsy.Data
{
    public interface IDbContextFactory
    {
        WeapsyDbContext Create();
    }
}
