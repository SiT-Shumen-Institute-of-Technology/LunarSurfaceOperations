namespace LunarSurfaceOperations.Core.Contracts.Processors.MessageAttribute
{
    using System.Threading;
    using System.Threading.Tasks;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts;
    using LunarSurfaceOperations.Utilities.OperationResults;

    public interface IMessageAttributeIntermediaryProcessor
    {
        Task<IOperationResult<IMessageAttributeLayout>> ConstructLayoutAsync(CancellationToken cancellationToken);
    }
}