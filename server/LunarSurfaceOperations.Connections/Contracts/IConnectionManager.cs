namespace LunarSurfaceOperations.Connections.Contracts
{
    using JetBrains.Annotations;

    public interface IConnectionManager<out TConnection>
    {
        [NotNull]
        TConnection Connection { get; }
    }
}