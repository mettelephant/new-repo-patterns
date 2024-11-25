namespace newsetup.repos.ApiService.Presentation.Kafka.Messages;

public class UserUpdatedMessage : ICustomerMessage, ICorrelatedMessage
{
    public Guid CustomerId { get; set; }
    public Guid CorrelationId { get; set; }
}