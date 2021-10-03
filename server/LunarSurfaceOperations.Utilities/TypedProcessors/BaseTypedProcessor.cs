namespace LunarSurfaceOperations.Utilities.TypedProcessors
{
    public abstract class BaseTypedProcessor<TImplementation, TBase, TResult> : ITypedProcessor<TBase, TResult>
        where TImplementation : class, TBase
    {
        public bool CanProcess(TBase entity) => entity is TImplementation;

        public TResult Process(TBase entity)
        {
            if (this.CanProcess(entity) == false)
                return default;

            var castedEntity = entity as TImplementation;
            return this.Process(castedEntity);
        }

        protected abstract TResult Process(TImplementation entity);
    }
}