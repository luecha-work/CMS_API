﻿using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CMS_API.HealthCheck
{
    public class ApiHealthCheck : IHealthCheck
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public ApiHealthCheck(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default
        )
        {
            var path = _configuration.GetSection("HealthChecks")["Path"];
            using (var httpClient = _httpClientFactory.CreateClient())
            {
                var response = await httpClient.GetAsync(path);
                if (response.IsSuccessStatusCode)
                {
                    return await Task.FromResult(
                        new HealthCheckResult(
                            status: HealthStatus.Healthy,
                            description: "The API is up and running."
                        )
                    );
                }
                return await Task.FromResult(
                    new HealthCheckResult(
                        status: HealthStatus.Unhealthy,
                        description: "The API is down."
                    )
                );
            }
        }
    }
}
