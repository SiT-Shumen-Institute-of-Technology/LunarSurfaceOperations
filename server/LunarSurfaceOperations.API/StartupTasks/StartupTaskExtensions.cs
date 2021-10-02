namespace LunarSurfaceOperations.API.StartupTasks
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Quantum.DMS.Utilities;

    public static class StartupTaskExtensions
    {
        public static Task ExecuteStartupTasksAsync([NotNull] this IWebHost host, CancellationToken cancellationToken = default)
        {
            if (host is null)
                throw new ArgumentNullException(nameof(host));

            return host.Services.ExecuteStartupTasksAsync(cancellationToken);
        }

        private static async Task ExecuteStartupTasksAsync([NotNull] this IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
        {
            if (serviceProvider is null)
                throw new ArgumentNullException(nameof(serviceProvider));

            var startupTasks = serviceProvider.GetServices<IStartupTask>();
            foreach (var startupTask in startupTasks.OrEmptyIfNull().IgnoreNullValues())
                await startupTask.ExecuteAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}