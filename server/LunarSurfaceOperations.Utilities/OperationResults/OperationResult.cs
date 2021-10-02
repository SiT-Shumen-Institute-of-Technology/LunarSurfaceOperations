namespace LunarSurfaceOperations.Utilities.OperationResults
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class OperationResult : IOperationResult
    {
        private readonly List<string> _errorMessages = new();

        public IReadOnlyCollection<string> Errors => this._errorMessages.AsReadOnly();

        public bool Success => this._errorMessages.Any() is false;

        public void AddErrorMessage(string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
                return;

            this._errorMessages.Add(errorMessage);
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            foreach (var errorMessage in this._errorMessages)
                stringBuilder.AppendLine(errorMessage);

            return stringBuilder.ToString();
        }
    }
    
    public class OperationResult<TEntity> : OperationResult, IOperationResult<TEntity>
    {
        public TEntity Data { get; set; }
    }
}