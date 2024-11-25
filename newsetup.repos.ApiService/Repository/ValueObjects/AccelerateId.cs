namespace newsetup.repos.ApiService.Repository.ValueObjects;

public record AccelerateId(Guid Value)
{
    public static implicit operator Guid(AccelerateId id) => id.Value;
    public Guid Id => this.Value;
}