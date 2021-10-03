namespace LunarSurfaceOperations.Core.OperativeModels.Layouts
{
    using System.Collections.Generic;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Layouts;
    using MongoDB.Bson;

    public record UpdateWorkspaceMembersLayout : IUpdateWorkspaceMembersLayout
    {
        private readonly List<ObjectId> _removedUsers = new();
        private readonly List<ObjectId> _newUsers = new();

        public UpdateWorkspaceMembersLayout(IWorkspaceLayout workspace)
        {
            this.Workspace = workspace;
        }

        public IWorkspaceLayout Workspace { get; }
        public IReadOnlyCollection<ObjectId> RemovedUsers => this._removedUsers.AsReadOnly();
        public IReadOnlyCollection<ObjectId> NewUsers => this._newUsers.AsReadOnly();

        public void AddRemovedUser(ObjectId removedUser) => this._removedUsers.Add(removedUser);
        public void AddNewUser(ObjectId newUser) => this._newUsers.Add(newUser);
    }
}