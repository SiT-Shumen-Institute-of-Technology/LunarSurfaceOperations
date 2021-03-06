namespace LunarSurfaceOperations.API
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using FluentValidation;
    using JetBrains.Annotations;
    using LunarSurfaceOperations.API.Converters;
    using LunarSurfaceOperations.API.Factories;
    using LunarSurfaceOperations.API.Factories.Contracts;
    using LunarSurfaceOperations.API.Factories.Contracts.MessageAttributes;
    using LunarSurfaceOperations.API.Factories.MessageAttributes;
    using LunarSurfaceOperations.API.Hubs;
    using LunarSurfaceOperations.API.Middlewares;
    using LunarSurfaceOperations.API.Settings;
    using LunarSurfaceOperations.API.StartupTasks;
    using LunarSurfaceOperations.Authentication;
    using LunarSurfaceOperations.Authentication.Contracts;
    using LunarSurfaceOperations.Configuration.Authentication;
    using LunarSurfaceOperations.Configuration.Database;
    using LunarSurfaceOperations.Connections.Contracts;
    using LunarSurfaceOperations.Core.Authentication;
    using LunarSurfaceOperations.Core.Contracts.Authentication;
    using LunarSurfaceOperations.Core.Contracts.OperativeModels.Prototypes;
    using LunarSurfaceOperations.Core.Contracts.Processors.MessageAttribute;
    using LunarSurfaceOperations.Core.Contracts.Services;
    using LunarSurfaceOperations.Core.OperativeModels.Prototypes;
    using LunarSurfaceOperations.Core.Processors.MessageAttribute;
    using LunarSurfaceOperations.Core.Services;
    using LunarSurfaceOperations.Core.Validators;
    using LunarSurfaceOperations.Data.Connections;
    using LunarSurfaceOperations.Data.Contracts;
    using LunarSurfaceOperations.Data.Models;
    using LunarSurfaceOperations.Data.Repositories;
    using LunarSurfaceOperations.Validation;
    using LunarSurfaceOperations.Validation.Contracts;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using MongoDB.Bson.Serialization;
    using MongoDB.Driver;
    using Quantum.DMS.Utilities;

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
            RegisterModels();

            services.AddSingleton<IStartupTask, SetupDatabaseStartupTask>();
            services.AddSingleton<IConnectionManager<IMongoDatabase>, MongoDatabaseConnection>();
            services.AddSingleton<IPasswordHashingService, PasswordHashingService>();
            services.AddScoped<IUserFactory, UserFactory>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMessageAttributeLayoutProcessor, MessageStringAttributeLayoutProcessor>();
            services.AddScoped<IMessageAttributeProcessor, MessageStringAttributeProcessor>();
            services.AddScoped<IMessageFactory, MessageFactory>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IWorkspaceService, WorkspaceService>();
            services.AddScoped<IWorkspaceRepository, WorkspaceRepository>();
            services.AddScoped<IWorkspaceFactory, WorkspaceFactory>();
            services.AddScoped<IAuthenticationContext, AuthenticationContext>();
            services.AddScoped(typeof(IExhaustiveValidator<>), typeof(ExhaustiveValidator<>));
            services.AddSingleton<IValidator<IUserPrototype>, UserValidator>();
            services.AddSingleton<IValidator<IWorkspacePrototype>, WorkspaceValidator>();
            services.AddScoped<IValidator<IMessagePrototype>, MessageValidator>();
            services.AddSingleton<IValidator<MessageStringAttributePrototype>, MessageStringAttributeValidator>();
            services.AddSingleton<IUserIdProvider, UserIdProvider>();

            services.Configure<DatabaseSettings>(this._configuration.GetSection(DatabaseSettings.Section));

            services.AddControllers(
                    options =>
                    {
                        options.ModelBinderProviders.Insert(0, new ObjectIdModelBinderProvider());
                    })
                .AddJsonOptions(
                    options =>
                    {
                        options.JsonSerializerOptions.Converters.Add(new ObjectIdJsonConverter());
                        options.JsonSerializerOptions.Converters.Add(new MessageAttributeInputModelConverter());
                        options.JsonSerializerOptions.Converters.Add(new MessageAttributeViewModelSerializer());
                    });
            services.AddSignalR(
                options =>
                {
                    options.AddFilter(new HubAuthenticationFilter());
                })
                .AddJsonProtocol(
                    options =>
                    {
                        options.PayloadSerializerOptions.Converters.Add(new ObjectIdJsonConverter());
                        options.PayloadSerializerOptions.Converters.Add(new MessageAttributeViewModelSerializer());
                    });
            
            this.ConfigureAuthentication(services);
            this.ConfigureCors(services);
        }

        private void ConfigureAuthentication(IServiceCollection services)
        {
            services.Configure<AuthenticationTokenGenerationSettings>(this._configuration.GetSection(AuthenticationTokenGenerationSettings.Section));
            services.AddSingleton<IAuthenticationTokenFactory, AuthenticationTokenFactory>();

            var tokenValidationSection = this._configuration.GetSection(AuthenticationTokenValidationSettings.Section);
            var tokenValidationSettings = tokenValidationSection.Get<AuthenticationTokenValidationSettings>();
            services.Configure<AuthenticationTokenValidationSettings>(tokenValidationSection);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(
                    options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            RequireExpirationTime = true,
                            ValidateLifetime = true,
                            ValidateIssuer = true,
                            ValidIssuers = tokenValidationSettings.Issuers.OrEmptyIfNull().IgnoreNullValues(),
                            RequireAudience = true,
                            ValidateAudience = true,
                            ValidAudiences = tokenValidationSettings.Audiences.OrEmptyIfNull().IgnoreNullValues(),

                            // NOTE: Tony Troeff, 02/08/2021 - This setup allows us to use asymmetric token encryption in future.
                            RequireSignedTokens = true,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKeys = AsSecurityKeys(tokenValidationSettings.IssuerSigningKeys),
                            TokenDecryptionKeys = AsSecurityKeys(tokenValidationSettings.DecryptionKeys),
                        };
                        
                        options.Events = new JwtBearerEvents
                        {
                            OnMessageReceived = context =>
                            {
                                if (context.Request.Path.ToString().StartsWith(RouteConstants.HubsPrefix))
                                    context.Token = context.Request.Query["access_token"];
                                return Task.CompletedTask;
                            },
                        };
                    });

            services.AddAuthorization(
                options =>
                {
                    var defaultPolicyBuilder = new AuthorizationPolicyBuilder();
                    defaultPolicyBuilder.RequireAuthenticatedUser();
                    var defaultPolicy = defaultPolicyBuilder.Build();

                    options.DefaultPolicy = defaultPolicy;
                    options.FallbackPolicy = defaultPolicy;
                });
        }

        private static IEnumerable<SymmetricSecurityKey> AsSecurityKeys(IEnumerable<string> data) => data.OrEmptyIfNull().IgnoreNullValues().Select(x => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(x)));

        private void ConfigureCors(IServiceCollection services)
        {
            var corsSettings = this._configuration.GetSection(CorsSettings.Section).Get<CorsSettings>();
            if (corsSettings is null)
                throw new InvalidOperationException("Specify CORS settings.");

            services.AddCors(
                options =>
                {
                    options.AddDefaultPolicy(
                        corsPolicyOptions =>
                        {
                            corsPolicyOptions.WithOrigins(corsSettings.AllowedOrigins.OrEmptyIfNull().ToArray());
                            corsPolicyOptions.WithHeaders(corsSettings.Headers.OrEmptyIfNull().ToArray());
                            corsPolicyOptions.WithMethods(corsSettings.Methods.OrEmptyIfNull().ToArray());
                            corsPolicyOptions.AllowCredentials();
                        });
                });
        }

        private static void RegisterModels()
        {
            BsonClassMap.RegisterClassMap<MessageStringAttribute>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<AuthenticationContextMiddleware>();

            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapHub<MessagesHub>($"{RouteConstants.HubsPrefix}/messages");
                });
        }
    }
}