namespace newsetup.repos.ApiService.Domain.HostedServices.Configuration;

public record ScopedBatchServiceOptions
{
    public int MaxRetryDelayMinutes { get; init; }
    public int BatchSize { get; set; }
    public int PollingInterval { get; set; }
}