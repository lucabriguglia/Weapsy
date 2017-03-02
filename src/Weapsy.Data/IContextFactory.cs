namespace Weapsy.Data
{
    public interface IContextFactory
    {
        WeapsyDbContext Create();
    }
}
