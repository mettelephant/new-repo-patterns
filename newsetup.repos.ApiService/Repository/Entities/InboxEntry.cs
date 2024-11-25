using newsetup.repos.ApiService.Uitlities;
using NodaTime;

namespace newsetup.repos.ApiService.Repository.Entities;

using System;

public abstract class InboxEntry<TMessage> where TMessage : class
{
    public Guid Id { get; set; }

    public required string MessageType { get; set; }
    
    public TMessage Message { get; set; }

    public Instant ReceivedAt { get; set; }
    public Instant? ProcessedAt { get; set; }
    public Instant? NotBefore { get; set; }
    public string? ErrorMessages { get; set; }
    public int AttemptCount { get; set; }
    public EntryStatus Status { get; set; }
}