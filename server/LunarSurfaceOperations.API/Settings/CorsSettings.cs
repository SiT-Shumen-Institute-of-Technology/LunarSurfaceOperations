namespace LunarSurfaceOperations.API.Settings
{
    using System.Collections.Generic;
    using JetBrains.Annotations;

    public class CorsSettings
    {
        public const string Section = "Cors";

        public IEnumerable<string> Headers { get; [UsedImplicitly] set; }

        public IEnumerable<string> Methods { get; [UsedImplicitly] set; }

        public IEnumerable<string> AllowedOrigins { get; [UsedImplicitly] set; }
    }
}