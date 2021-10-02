namespace LunarSurfaceOperations.Core.Services.ScopeIdentification
{
    using LunarSurfaceOperations.Data.Contracts;

    public class EmptyScopeIdentification<TEntity> : IScopeIdentification<TEntity>
        where TEntity : IEntity
    {
        public void Apply(TEntity entity)
        {
        }
    }
}