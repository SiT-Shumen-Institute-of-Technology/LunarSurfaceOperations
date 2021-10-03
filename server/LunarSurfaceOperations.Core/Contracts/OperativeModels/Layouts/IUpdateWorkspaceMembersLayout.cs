namespace LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts
{
    using System.Collections.Generic;
    using MongoDB.Bson;

    public interface IUpdateWorkspaceMembersLayout
    {
        IReadOnlyCollection<ObjectId> RemovedUsers { get; }
        IReadOnlyCollection<ObjectId> NewUsers { get; }
        IWorkspaceLayout Workspace { get; }
    }
}