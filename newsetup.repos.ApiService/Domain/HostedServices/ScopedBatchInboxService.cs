using AutoMapper;
using newsetup.repos.ApiService.Domain.HostedServices.Configuration;
using newsetup.repos.ApiService.Domain.Models;
using newsetup.repos.ApiService.Domain.Services;
using newsetup.repos.ApiService.Repository.Entities;
using newsetup.repos.ApiService.Repository.Interfaces;

namespace newsetup.repos.ApiService.Domain.HostedServices;

// Base inbox processing service with proper DI and generic message type
public abstract class ScopedBatchInboxService<TMessage, TInboxMessage, TInboxService> : BackgroundService
    where TMessage : class
    where TInboxMessage : InboxMessage<TMessage>
    where TInboxService : IInboxService<TMessage>
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ScopedBatchInboxService<TMessage, TInboxMessage, TInboxService>> _logger;
    private readonly ScopedBatchServiceOptions _options;
    private readonly IMapper _mapper;
    
    protected int PollingInterval => _options.PollingInterval;
    protected int BatchSize => _options.PollingInterval;

    protected ScopedBatchInboxService(
        IServiceScopeFactory scopeFactory,
        ILogger<ScopedBatchInboxService<TMessage, TInboxMessage, TInboxService>> logger,
        ScopedBatchServiceOptions options,
        IMapper mapper)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _options = options;
        _mapper = mapper;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessPendingMessages(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing inbox messages");
            }

            await Task.Delay(PollingInterval, stoppingToken);
        }
    }

    private async Task ProcessPendingMessages(CancellationToken stoppingToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var inboxRepository = scope.ServiceProvider.GetRequiredService<IInboxRepository>();
        var handler = scope.ServiceProvider.GetRequiredService<TInboxService>();

        var messageEntities = await inboxRepository
            .GetBatchInboxMessages<TInboxMessage>(BatchSize);
        var messages = _mapper.Map<IEnumerable<TInboxMessage>>(messageEntities);

        foreach (var message in messages)
        {
            if (stoppingToken.IsCancellationRequested)
            {
                break;
            }

            try
            {
                await ProcessMessage(message, inboxRepository, handler);
            }
            catch (Exception ex)
            {
                await HandleProcessingError(message, ex, inboxRepository);
            }
        }
    }

    private async Task ProcessMessage(TInboxMessage message, IInboxRepository dbContext, TInboxService service)
    {
        var messageEntity = _mapper.Map<InboxEntry<TMessage>>(message);
        await dbContext.MarkInboxMessageAsInProgress(messageEntity);
        
        if(message.Message is null)
        {
            throw new InvalidOperationException("Message is null");
        }

        await service.PreExecute(message.Message);
        await service.Execute(message.Message);
        await service.PostExecute(message.Message);
        
        messageEntity = _mapper.Map<InboxEntry<TMessage>>(message);
        await dbContext.MarkInboxMessageAsProcessed(messageEntity);
    }

    private async Task HandleProcessingError(TInboxMessage message, Exception ex, IInboxRepository dbContext)
    {
        var messageEntity = _mapper.Map<InboxEntry<TMessage>>(message);
        await dbContext.MarkInboxMessageAsFailed(messageEntity, ex, _options.MaxRetryDelayMinutes);
    }
}