using AutoMapper;
using MassTransit;
using newsetup.repos.ApiService.Domain.Models;
using newsetup.repos.ApiService.Repository.Entities;
using newsetup.repos.ApiService.Repository.Interfaces;
using newsetup.repos.ApiService.Uitlities;
using NodaTime;

namespace newsetup.repos.ApiService.Presentation.Kafka.Consumers;

// Base consumer that implements the inbox pattern
public abstract class InboxConsumer<TMessage, TInboxMessage>(
    IInboxRepository inboxRepository,
    IClock clock,
    IMapper mapper)
    : IConsumer<TMessage>
    where TMessage : class
    where TInboxMessage : InboxMessage<TMessage>, new()
{
    public virtual async Task Consume(ConsumeContext<TMessage> context)
    {
        var messageId = context.MessageId ?? Guid.NewGuid();
        var message = CreateInboxMessage(messageId, context.Message);
        
        message.ReceivedAt = clock.GetCurrentInstant();
        message.Status = EntryStatus.Pending;
        message.MessageType = typeof(TMessage).FullName ?? throw new InvalidOperationException();

        var messageEntity = mapper.Map<InboxEntry<TMessage>>(message);
        await inboxRepository.AddInboxMessage(messageEntity);
    }

    // Default implementation that can be overridden if needed
    protected virtual TInboxMessage CreateInboxMessage(Guid messageId, TMessage message) =>
        new()
        {
            Id = messageId,
            Message = message
        };
}