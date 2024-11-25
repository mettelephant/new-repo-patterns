namespace newsetup.repos.ApiService.Domain.Services;

// Base message handler interface
public interface IInboxService<in TMessage>
    where TMessage : class
{
    Task PreExecute(TMessage message) => Task.CompletedTask;
    Task Execute(TMessage message);
    Task PostExecute(TMessage message) => Task.CompletedTask;
}