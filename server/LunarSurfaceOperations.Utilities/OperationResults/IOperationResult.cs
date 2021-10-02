namespace LunarSurfaceOperations.Utilities.OperationResults
{
    using System.Collections.Generic;

    public interface IOperationResult
    {
        IReadOnlyCollection<string> Errors { get; }
        bool Success { get; }

        void AddErrorMessage(string errorMessage);
    }
    
    public interface IOperationResult<TEntity> : IOperationResult
    {
        TEntity Data { get; set; }
    }
}