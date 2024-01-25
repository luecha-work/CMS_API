using System.Data;
using AspNetCoreRateLimit;
using Authentication.Service.Contracts;
using CMS_API.ContextFactory;
using CMS_API.Core.Middleware;
using CMS_API.Extensions;
using CMS_API.HealthCheck;
using Entities;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;
using Repository.EntityFramework;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// builder
//     .Services
//     .AddDbContext<CMSDevDbContext>(options =>
//     {
//         options.UseNpgsql(connectionString);
//     });

builder
    .Services
    .AddDbContext<CMSDevDbContext>(
        options =>
        {
            options.UseNpgsql(connectionString);
        },
        ServiceLifetime.Scoped,
        ServiceLifetime.Singleton
    );

builder
    .Host
    .UseSerilog((ctx, lc) => lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));

builder.Services.ConfigureCors(builder.Configuration);
builder.Services.ConfigureIISIntegration();
builder.Services.AddMemoryCache();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureSession(builder.Configuration);
builder.Services.ConfigureRateLimitingOptions();
builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureVersioning();
builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.ConfigureIdentityCore(builder.Configuration);
builder.Services.ConfigureHangfireJob(builder.Configuration);
builder.Services.AddAuthentication();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddLogging();
builder.Services.ConfigureServiceManager();
builder.Services.AddJwtConfiguration(builder.Configuration);
builder.Services.AddAuthProviderConfiguration(builder.Configuration);

// TODO: FOR Authentication
builder.Services.AddScoped<IAuthServiceManager, Authentication.Service.AuthServiceManager>();
builder
    .Services
    .AddScoped<
        Authentication.Contracts.IAuthRepositoryManager,
        Authentication.Repository.AuthRepositoryManager
    >();

//TODO: Add Scoped
// builder.Services.AddScoped<Repository.RepositoryManager>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<Repository.EntityFramework.RepositoryManager>();
builder.Services.AddScoped<IRepositoryFactory, RepositoryFactory>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

/*builder.Services.AddSwaggerGen();*/

builder
    .Services
    .AddHealthChecks()
    .AddCheck<ServiceHealthCheck>(nameof(ServiceHealthCheck))
    .AddCheck<DbHealthCheck>(nameof(DbHealthCheck))
    .AddCheck<ApiHealthCheck>(nameof(ApiHealthCheck));

builder.Services.AddHealthChecksUI().AddInMemoryStorage();

var app = builder.Build();

if (app.Environment.IsProduction())
{
    app.UseHsts();
    app.Use(
        async (context, next) =>
        {
            // Set Content Security Policy (CSP) to limit allowed resources sources
            context.Response.Headers.Add("Content-Security-Policy", "default-src 'self';");

            // Call the next middleware in the pipeline
            await next();
        }
    );
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Use(
    async (context, next) =>
    {
        // Set X-Frame-Options header to prevent web pages from being embedded in iframes from different sources
        context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");

        // Enable XSS (Cross-Site Scripting) protection by setting the X-XSS-Protection header
        context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");

        // Set X-Content-Type-Options header to prevent browsers from interpreting files as something else than declared in the content
        context.Response.Headers.Add("X-Content-Type-Options", "nosniff");

        // Set Referrer-Policy header to specify how much referrer information to include with requests
        context.Response.Headers.Add("Referrer-Policy", "strict-origin");

        // Call the next middleware in the pipeline
        await next();
    }
);

app.UseSession();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.All });

app.UseIpRateLimiting();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();

/*app.UseMiddleware<JwtUserProviderMiddleware>();
app.UseMiddleware<JwtAuthorizationMiddlewareExtensions>(app);
app.UseMiddleware<EventIDMiddlewareExtensions>();*/
app.MapControllers();
app.MapHealthChecks(
    "/healthcheck",
    new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    }
);

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecksUI();

app.Run();

/*void SeedDatabase1()
{
    using (var scope = app.Services.CreateScope())
    {
        try
        {
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<CMSDevDbContext>();
            dbContext.Database.EnsureCreated();
            dbContext.SeedData1();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while seeding database 1: {ex.Message}");
        }
    }
}*/

/*void SeedDatabase2()
{
    using (var scope = app.Services.CreateScope())
    {
        try
        {
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<CMSDevDbContext>();
            dbContext.Database.EnsureCreated();
            dbContext.SeedData2();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while seeding database 2: {ex.Message}");
        }
    }
}*/
