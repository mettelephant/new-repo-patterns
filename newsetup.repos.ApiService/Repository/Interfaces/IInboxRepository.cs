using newsetup.repos.ApiService.Repository.Entities;

namespace newsetup.repos.ApiService.Repository.Interfaces;

public interface IInboxRepository
{
    public Task<bool> AddInboxMessage<TMessage>(InboxEntry<TMessage> message)
        where TMessage : class;

    public Task<IEnumerable<InboxEntry<TMessage>>> GetBatchInboxMessages<TMessage>(int batch)
        where TMessage : class;

    public Task<bool> MarkInboxMessageAsInProgress<TMessage>(InboxEntry<TMessage> message)
        where TMessage : class;
    public Task<bool> MarkInboxMessageAsProcessed<TMessage>(InboxEntry<TMessage> message)
        where TMessage : class;
    public Task<bool> MarkInboxMessageAsFailed<TMessage>(InboxEntry<TMessage> message,
        Exception exception,
        int retryDelayMinutes)
        where TMessage : class;
}