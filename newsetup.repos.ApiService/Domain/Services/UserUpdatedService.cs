using newsetup.repos.ApiService.Domain.Events;

namespace newsetup.repos.ApiService.Domain.Services;

public class UserUpdatedService : IInboxService<UserUpdatedEvent>
{
    public Task Execute(UserUpdatedEvent @event)
    {
        throw new NotImplementedException();
    }
}