namespace LunarSurfaceOperations.Configuration.Database
{
    public class DatabaseSettings
    {
        public static string Section = "DatabaseSettings";

        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}