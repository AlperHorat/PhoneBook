using PhoneBook.Domain.Entities;

namespace PhoneBook.Services.ContactInfos;

public interface IContactInfoService
{
    Task<List<ContactInfo>> GetByContactIdAsync(Guid contactId);
    Task<ContactInfo> CreateAsync(ContactInfo contactInfo);
    Task DeleteAsync(Guid id);
    Task SoftDeleteAsync(Guid id);
}