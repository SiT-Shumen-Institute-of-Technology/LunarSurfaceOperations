namespace LunarSurfaceOperations.Core.Services.ScopeIdentification
{
    public interface IScopeIdentification<in TEntity>
    {
        void Apply(TEntity entity);
    }
}