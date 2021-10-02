﻿namespace LunarSurfaceOperations.Utilities.OperationResults
{
    using System.Collections.Generic;

    public interface IOperationResult
    {
        IReadOnlyCollection<string> Errors { get; }

        void AddErrorMessage(string errorMessage);
    }
    
    public interface IOperationResult<TEntity>
    {
        TEntity Data { get; set; }
    }
}