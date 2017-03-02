namespace Weapsy.Apps.Text.Data
{
    public interface IContextFactory
    {
        TextDbContext Create();
    }
}
