using PhoneBook.Domain.Entities;

namespace PhoneBook.Services.Contacts;

public interface IContactService
{
    Task<List<Contact>> GetAllAsync();
    Task<Contact?> GetByIdAsync(Guid id);
    Task<Contact> CreateAsync(Contact contact);
    Task DeleteAsync(Guid id);
}