namespace LunarSurfaceOperations.API.StartupTasks
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IStartupTask
    {
        Task ExecuteAsync(CancellationToken cancellationToken = default);
    }
}