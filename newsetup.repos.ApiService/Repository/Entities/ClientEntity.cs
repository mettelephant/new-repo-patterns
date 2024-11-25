namespace newsetup.repos.ApiService.Repository.Entities;

public class ClientEntity
{
    public Guid ClientId { get; set; }
    public Guid AccelerateId { get; set; }
    public string DatabaseName { get; set; }
    public string Server { get; set; }
    public bool IsActive { get; set; }
}