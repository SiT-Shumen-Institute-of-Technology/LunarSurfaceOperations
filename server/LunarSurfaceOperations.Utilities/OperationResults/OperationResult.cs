namespace LunarSurfaceOperations.Utilities.OperationResults
{
    using System.Collections.Generic;

    public class OperationResult : IOperationResult
    {
        private readonly List<string> _errorMessages = new();

        public IReadOnlyCollection<string> Errors => this._errorMessages.AsReadOnly();

        public void AddErrorMessage(string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
                return;

            this._errorMessages.Add(errorMessage);
        }
    }
    
    public class OperationResult<TEntity> : OperationResult, IOperationResult<TEntity>
    {
        public TEntity Data { get; set; }
    }
}