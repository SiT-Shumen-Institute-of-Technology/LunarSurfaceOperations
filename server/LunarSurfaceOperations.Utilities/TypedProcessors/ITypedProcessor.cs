namespace LunarSurfaceOperations.Utilities.TypedProcessors
{
    public interface ITypedProcessor<in TBase, out TResult>
    {
        bool CanProcess(TBase entity);
        TResult Process(TBase entity);
    }
}