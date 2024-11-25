using Microsoft.EntityFrameworkCore;
using newsetup.repos.ApiService.Repository.Entities;
using newsetup.repos.ApiService.Repository.Interfaces;
using newsetup.repos.ApiService.Uitlities;
using NodaTime;

namespace newsetup.repos.ApiService.Repository.Concrete;

public class InboxRepository : IInboxRepository
{
    private readonly IClock _clock;
    private readonly NewRepoContext _dbContext;

    public InboxRepository(IClock clock, NewRepoContext dbContext)
    {
        _clock = clock;
        _dbContext = dbContext;
    }

    public async Task<bool> AddInboxMessage<TMessage>(InboxEntry<TMessage> message)
        where TMessage : class
    {
        await _dbContext.Set<InboxEntry<TMessage>>()
            .AddAsync(message);

        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<InboxEntry<TMessage>>> GetBatchInboxMessages<TMessage>(int batch)
        where TMessage : class
    {
        var currentInstant = _clock.GetCurrentInstant();
        
        return await _dbContext.Set<InboxEntry<TMessage>>()
            .Where(m => m.Status != EntryStatus.Processed
                        && m.Status != EntryStatus.InProcess
                        && (m.NotBefore == null || m.NotBefore <= currentInstant))
            .OrderBy(m => m.ReceivedAt)
            .Take(batch)
            .ToListAsync();
    }

    public async Task<bool> MarkInboxMessageAsInProgress<TMessage>(InboxEntry<TMessage> message)
        where TMessage : class
    {
        var inboxEntry = await _dbContext.Set<InboxEntry<TMessage>>()
            .FirstOrDefaultAsync(m => m.Id == message.Id);

        if (inboxEntry == null)
            return false;

        inboxEntry.Status = EntryStatus.InProcess;
        _dbContext.Entry(inboxEntry).State = EntityState.Modified;

        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> MarkInboxMessageAsProcessed<TMessage>(InboxEntry<TMessage> message) where TMessage : class
    {
        var inboxEntry = await _dbContext.Set<InboxEntry<TMessage>>()
            .FirstOrDefaultAsync(m => m.Id == message.Id);

        if (inboxEntry == null)
            return false;

        inboxEntry.Status = EntryStatus.Processed;
        _dbContext.Entry(inboxEntry).State = EntityState.Modified;

        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> MarkInboxMessageAsFailed<TMessage>(InboxEntry<TMessage> message,
        Exception exception,
        int retryDelayMinutes) where TMessage : class
    {
        var inboxEntry = await _dbContext.Set<InboxEntry<TMessage>>()
            .FirstOrDefaultAsync(m => m.Id == message.Id);

        if (inboxEntry == null)
            return false;

        inboxEntry.Status = EntryStatus.Failed;
        inboxEntry.AttemptCount++;
        inboxEntry.ErrorMessages = string.IsNullOrEmpty(inboxEntry.ErrorMessages)
            ? exception.Message
            : $"{inboxEntry.ErrorMessages}\n{exception.Message}";

        var delayMinutes = Math.Min(Math.Pow(2, inboxEntry.AttemptCount), retryDelayMinutes);
        inboxEntry.NotBefore = _clock.GetCurrentInstant().Plus(Duration.FromMinutes(delayMinutes));
        _dbContext.Entry(inboxEntry).State = EntityState.Modified;

        await _dbContext.SaveChangesAsync();
        return true;
    }
}