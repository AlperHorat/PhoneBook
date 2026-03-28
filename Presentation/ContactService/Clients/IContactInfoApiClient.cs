using ContactService.Dtos;

namespace ContactService.Clients;

public interface IContactInfoApiClient
{
    Task<List<ContactInfoResponse>> GetByContactIdAsync(Guid contactId);
}