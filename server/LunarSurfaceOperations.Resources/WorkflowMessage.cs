namespace LunarSurfaceOperations.Resources
{
    public static class WorkflowMessages
    {
        public static string UpdateHasNoMatches => "The update operations did not execute successfully because the requested resource was not found.";
        public static string AuthenticationIsRequired => "You need to be authenticated in order to execute this operation.";
        public static string UsernameAlreadyTaken => "This username is already taken.";
        public static string EntityNotFound => "The requested resource was not found.";
        public static string UserIsNotMemberOfWorkspace => "You are not a member of the requested workspace.";
        public static string UserIsNotOwnerOfWorkspace => "You need to be the owner of a workspace to execute the requested operation.";
    }
}