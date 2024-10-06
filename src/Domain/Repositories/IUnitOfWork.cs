namespace Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> Commit(CancellationToken cancellationToken);
    }
}
