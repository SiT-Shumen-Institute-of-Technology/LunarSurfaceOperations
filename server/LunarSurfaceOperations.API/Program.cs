namespace LunarSurfaceOperations.API
{
    using System.Threading.Tasks;
    using LunarSurfaceOperations.API.StartupTasks;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;

    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var webHost = CreateHostBuilder(args).Build();
            await webHost.ExecuteStartupTasksAsync();
            await webHost.RunAsync();
        }

        private static IWebHostBuilder CreateHostBuilder(string[] args) => WebHost.CreateDefaultBuilder<Startup>(args);
    }
}