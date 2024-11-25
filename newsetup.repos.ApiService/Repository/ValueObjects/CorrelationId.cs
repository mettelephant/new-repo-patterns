namespace newsetup.repos.ApiService.Repository.ValueObjects;

public record CorrelationId(Guid Value)
{
    public static implicit operator Guid(CorrelationId id) => id.Value;
    public Guid Id => Value;
}