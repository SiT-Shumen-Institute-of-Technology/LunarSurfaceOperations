namespace LunarSurfaceOperations.API.ViewModels.Workspace
{
    using System.Collections.Generic;
    using JetBrains.Annotations;

    public class UpdateWorkspaceMembersRequest
    {
        public IEnumerable<string> Members { get; [UsedImplicitly] set; }
    }
}