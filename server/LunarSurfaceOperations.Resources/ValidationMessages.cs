namespace LunarSurfaceOperations.Resources
{
    using System.Resources;

    public static class ValidationMessages
    {
        public static string InvalidNullArgument => "A required argument is not provided.";
        public static string InvalidStringArgument => "A required text argument is not provided.";
        public static string InvalidRequest =>  "The sent request is not in a valid format.";
    }
}