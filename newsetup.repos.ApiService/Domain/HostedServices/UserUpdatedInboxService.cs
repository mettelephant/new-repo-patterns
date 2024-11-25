using AutoMapper;
using newsetup.repos.ApiService.Domain.Events;
using newsetup.repos.ApiService.Domain.HostedServices.Configuration;
using newsetup.repos.ApiService.Domain.Models;
using newsetup.repos.ApiService.Domain.Services;

namespace newsetup.repos.ApiService.Domain.HostedServices;

public class UserUpdatedInboxService(
    IServiceScopeFactory scopeFactory,
    ILogger<ScopedBatchInboxService<UserUpdatedEvent, InboxMessage<UserUpdatedEvent>, UserUpdatedService>> logger,
    ScopedBatchServiceOptions options,
    IMapper mapper,
    UserUpdatedServiceOptions userUpdatedServiceOptions)
    : ScopedBatchInboxService<UserUpdatedEvent, InboxMessage<UserUpdatedEvent>, UserUpdatedService>(scopeFactory,
        logger, options, mapper)
{
    protected new int PollingInterval => userUpdatedServiceOptions.PollingInterval;
}