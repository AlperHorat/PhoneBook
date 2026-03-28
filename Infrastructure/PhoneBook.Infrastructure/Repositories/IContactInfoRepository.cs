using PhoneBook.Domain.Entities;

namespace PhoneBook.Infrastructure.Repositories;

public interface IContactInfoRepository
{
    Task<List<ContactInfo>> GetByContactIdAsync(Guid contactId);
    Task<ContactInfo?> GetByIdAsync(Guid id);
    Task AddAsync(ContactInfo contactInfo);
    Task DeleteAsync(Guid id);
    Task SoftDeleteAsync(Guid id);
}