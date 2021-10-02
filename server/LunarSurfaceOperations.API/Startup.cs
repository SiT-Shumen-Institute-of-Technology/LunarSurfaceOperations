namespace LunarSurfaceOperations.API
{
    using System;
    using JetBrains.Annotations;
    using LunarSurfaceOperations.Authentication;
    using LunarSurfaceOperations.Authentication.Contracts;
    using LunarSurfaceOperations.Configuration.Authentication;
    using LunarSurfaceOperations.Configuration.Database;
    using LunarSurfaceOperations.Connections.Contracts;
    using LunarSurfaceOperations.Core.Contracts.Services;
    using LunarSurfaceOperations.Core.Services;
    using LunarSurfaceOperations.Data.Connections;
    using LunarSurfaceOperations.Data.Contracts;
    using LunarSurfaceOperations.Data.Repositories;
    using LunarSurfaceOperations.Validation;
    using LunarSurfaceOperations.Validation.Contracts;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using MongoDB.Driver;

    public class Startup
    {
        [NotNull]
        private readonly IConfiguration _configuration;

        public Startup([NotNull] IConfiguration configuration)
        {
            this._configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConnectionManager<IMongoDatabase>, MongoDatabaseConnection>();
            services.AddSingleton<IPasswordHashingService, PasswordHashingService>();
            services.AddSingleton<IAuthenticationTokenFactory, AuthenticationTokenFactory>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped(typeof(IExhaustiveValidator<>), typeof(ExhaustiveFluentValidator<>));

            services.Configure<DatabaseSettings>(this._configuration.GetSection(DatabaseSettings.Section));
            services.Configure<AuthenticationTokenGenerationSettings>(this._configuration.GetSection(AuthenticationTokenGenerationSettings.Section));
            services.Configure<AuthenticationTokenValidationSettings>(this._configuration.GetSection(AuthenticationTokenValidationSettings.Section));

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapControllers();
                });
        }
    }
}