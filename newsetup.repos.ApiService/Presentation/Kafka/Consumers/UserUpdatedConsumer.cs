using AutoMapper;
using newsetup.repos.ApiService.Domain.Models;
using newsetup.repos.ApiService.Presentation.Kafka.Messages;
using newsetup.repos.ApiService.Repository.Interfaces;
using NodaTime;

namespace newsetup.repos.ApiService.Presentation.Kafka.Consumers;

public class UserUpdatedConsumer(IInboxRepository inboxRepository, IClock clock, IMapper mapper)
    : InboxConsumer<UserUpdatedMessage, InboxMessage<UserUpdatedMessage>>(inboxRepository, clock, mapper);