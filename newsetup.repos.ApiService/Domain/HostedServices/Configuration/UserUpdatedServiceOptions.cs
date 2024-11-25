namespace newsetup.repos.ApiService.Domain.HostedServices.Configuration;

public record UserUpdatedServiceOptions
{
    public int PollingInterval { get; set; }
}