namespace newsetup.repos.ApiService.Presentation.Kafka.Messages;

public interface ICorrelatedMessage
{
    Guid CorrelationId { get; set; }
}