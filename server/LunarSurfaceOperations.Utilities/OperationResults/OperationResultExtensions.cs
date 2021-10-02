namespace LunarSurfaceOperations.Utilities.OperationResults
{
    using System;
    using LunarSurfaceOperations.Resources;
    using Quantum.DMS.Utilities;

    public static class OperationResultExtensions
    {
        public static void AddException(this IOperationResult operationResult, Exception exception)
        {
            if (operationResult is null || exception is null)
                return;

            operationResult.AddErrorMessage(exception.Message);
        }

        public static void ValidateNotNull<T>(this IOperationResult operationResult, T value)
            where T : class
        {
            if (operationResult is null)
                return;

            if (value is null)
                operationResult.AddErrorMessage(ValidationMessages.InvalidNullArgument);
        }

        public static T AppendErrorMessages<T>(this T originalOperationResult, params IOperationResult[] operationResultsToCombine)
            where T : IOperationResult
        {
            if (originalOperationResult is null)
                return default;
            
            foreach (var operationResult in operationResultsToCombine.OrEmptyIfNull().IgnoreNullValues())
                foreach (var errorMessage in operationResult.Errors.OrEmptyIfNull().IgnoreNullOrWhitespaceValues())
                    originalOperationResult.AddErrorMessage(errorMessage);

            return originalOperationResult;
        }
    }
}