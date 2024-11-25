namespace newsetup.repos.ApiService.Presentation.Kafka.Messages;

public interface ICustomerMessage
{
    Guid CustomerId { get; set; }
}