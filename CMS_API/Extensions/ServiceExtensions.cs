using System.Text;
using AspNetCoreRateLimit;
using Azure;
using Entities;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Service;
using Service.Contracts;

namespace CMS_API.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(
            this IServiceCollection services,
            IConfiguration configuration
        ) =>
            services.AddCors(options =>
            {
                var allowOrigin = configuration
                    .GetSection("CorsPolicySettings")["AllowedOrigins"]
                    .Split(",", StringSplitOptions.RemoveEmptyEntries);

                options.AddPolicy(
                    "CorsPolicy",
                    builder =>
                    {
                        builder
                            .WithOrigins(allowOrigin)
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials()
                            .WithExposedHeaders("X-Pagination");
                    }
                );
            });

        public static void ConfigureSession(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(1);
                options.Cookie.IsEssential = true;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.HttpOnly = true;
                options.Cookie.Name = "CMS_API.AspNetCore.Session";
            });
            /* services.AddSession(options =>
             {
                 options.IdleTimeout = TimeSpan.FromDays(1);
                 options.Cookie.IsEssential = true;
                 options.Cookie.SameSite = SameSiteMode.None;
                 options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                 options.Cookie.HttpOnly = true;
                 options.Cookie.Name = "CMS_API.AspNetCore.Session";
             });*/
        }

        public static void ConfigureRateLimitingOptions(this IServiceCollection services)
        {
            var rateLimitRules = new List<RateLimitRule>
            {
                new RateLimitRule
                {
                    Endpoint = "*",
                    Limit = 1000,
                    Period = "1m"
                }
            };
            services.Configure<IpRateLimitOptions>(opt =>
            {
                opt.GeneralRules = rateLimitRules;
            });

            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        }

        public static void ConfigureVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(opt =>
            {
                opt.ReportApiVersions = true;
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });
        }

        public static void ConfigureIISIntegration(this IServiceCollection services) =>
            services.Configure<IISOptions>(options => { });

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Title = "NewCMS API",
                        Version = "v1",
                        Description = "NewCMS API by Axonstech",
                    }
                );
                options.AddSecurityDefinition(
                    JwtBearerDefaults.AuthenticationScheme,
                    new OpenApiSecurityScheme
                    {
                        Description =
                            @"JWT Authorization header using the Bearer scheme.
                            Enter 'Bearer' [space] and then your token in the text input below.
                            Example: 'Bearer 12345abcdef'",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        Scheme = JwtBearerDefaults.AuthenticationScheme
                    }
                );

                options.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = JwtBearerDefaults.AuthenticationScheme
                                },
                                Scheme = "oauth2",
                                Name = JwtBearerDefaults.AuthenticationScheme,
                                In = ParameterLocation.Header
                            },
                            new List<string>()
                        }
                    }
                );
            });
        }

        public static void ConfigureJWT(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            var jwtConfiguration = new Authentication.Models.ConfigurationModels.JwtConfiguration();
            configuration.Bind(jwtConfiguration.Section, jwtConfiguration);
            var secretKey = jwtConfiguration.SecretKey;

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = jwtConfiguration.ValidIssuer,
                        ValidAudience = jwtConfiguration.ValidAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(secretKey)
                        )
                    };
                });
        }

        public static void ConfigureIdentityCore(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            var identityConfigure =
                new Authentication.Models.ConfigurationModels.IdentityProviderConfigure();
            configuration.Bind(identityConfigure.Section, identityConfigure);

            services
                .AddIdentityCore<Account>(options =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                })
                .AddRoles<Roles>()
                .AddTokenProvider<DataProtectorTokenProvider<Account>>(
                    identityConfigure.LoginProvider
                )
                .AddEntityFrameworkStores<CMSDevDbContext>()
                .AddDefaultTokenProviders();
        }

        public static void ConfigureHangfireJob(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            var connectionStringStorage = configuration.GetSection("HangfireSettings")[
                "ConnectionStringStorage"
            ];
            services.AddHangfire(options =>
            {
                options.UsePostgreSqlStorage(
                    connectionStringStorage,
                    new PostgreSqlStorageOptions
                    {
                        DistributedLockTimeout = TimeSpan.FromMinutes(1),
                        QueuePollInterval = TimeSpan.FromSeconds(15),
                        JobExpirationCheckInterval = TimeSpan.FromHours(1),
                        CountersAggregateInterval = TimeSpan.FromMinutes(5),
                        PrepareSchemaIfNecessary = false,
                        SchemaName = "public"
                    }
                );
                options.UseSerializerSettings(
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }
                );
            });
            services.AddHangfireServer();
        }

        public static void ConfigureServiceManager(this IServiceCollection services) =>
            services.AddScoped<IServiceManager, ServiceManager>();

        public static void AddJwtConfiguration(
            this IServiceCollection services,
            IConfiguration configuration
        ) =>
            services.Configure<Authentication.Models.ConfigurationModels.JwtConfiguration>(
                configuration.GetSection("JwtSettings")
            );

        public static void AddAuthProviderConfiguration(
            this IServiceCollection services,
            IConfiguration configuration
        ) =>
            services.Configure<Authentication.Models.ConfigurationModels.IdentityProviderConfigure>(
                configuration.GetSection("AuthProvider")
            );
        /*
           public static void ConfigureLoggerService(this IServiceCollection services) =>
               services.AddSingleton<ILoggerManager, LoggerManager>();
   
           public static void AddSmtpConfiguration(this IServiceCollection services, IConfiguration configuration) =>
               services.Configure<SmtpConfiguration>(configuration.GetSection("SmtpSettings"));
   
           public static void AddSmtpConfigurationForAuthen(this IServiceCollection services, IConfiguration configuration) =>
               services.Configure<Authentication.Entities.ConfigurationModels.SmtpConfiguration>(configuration.GetSection("SmtpSettings"));
   
           public static void AddAWSS3Configuration(this IServiceCollection services, IConfiguration configuration) =>
               services.Configure<AwsS3Configuration>(configuration.GetSection("AWSS3Settings"));
   
           public static void AddPowerBIConfiguration(this IServiceCollection services, IConfiguration configuration) =>
              services.Configure<PowerBIConfiguration>(configuration.GetSection("PowerBISettings"));
   
           public static void AddAzureOpenAIConfiguration(this IServiceCollection services, IConfiguration configuration) =>
              services.Configure<AzureOpenAIConfiguration>(configuration.GetSection("AzureOpenAISettings"));
   
           #region Elasticsearch
           public static void AddElasticsearchConfiguration(this IServiceCollection services, IConfiguration configuration) =>
             services.Configure<ElasticsearchConfig>(configuration.GetSection("ElasticSearchSettings"));
   
           public static void ConfigureElasticsearch(this IServiceCollection services, IConfiguration configuration)
           {
               var elasticConfigurtion = configuration.GetSection("ElasticSearchSettings").Get<ElasticsearchConfig>();
               services.AddElasticsearch(config =>
               {
                   config.CloudId = elasticConfigurtion.CloudId;
                   config.ApiKey = elasticConfigurtion.ApiKey;
               });
           }
           #endregion
   
           public static void AddGoogleVisionConfiguration(this IServiceCollection services, IConfiguration configuration) =>
              services.Configure<GoogleVisionConfiguration>(configuration.GetSection("GoogleVisionSettings"));*/
    }
}
