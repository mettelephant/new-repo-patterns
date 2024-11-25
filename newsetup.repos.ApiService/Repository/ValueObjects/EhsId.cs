namespace newsetup.repos.ApiService.Repository.ValueObjects;

public record EhsId(Guid Value)
{
    public static implicit operator Guid(EhsId id) => id.Value;
    public Guid Id => Value;
}