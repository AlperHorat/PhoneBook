using PhoneBook.Domain.Entities;
using PhoneBook.Infrastructure.Repositories;

namespace PhoneBook.Services.Contacts;

public class ContactService : IContactService
{
    private readonly IContactRepository _contactRepository;

    public ContactService(IContactRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }

    public async Task<List<Contact>> GetAllAsync()
    {
        return await _contactRepository.GetAllAsync();
    }

    public async Task<Contact?> GetByIdAsync(Guid id)
    {
        return await _contactRepository.GetByIdAsync(id);
    }

    public async Task<Contact> CreateAsync(Contact contact)
    {
        if (contact is null)
            throw new ArgumentNullException(nameof(contact));

        await _contactRepository.AddAsync(contact);
        return contact;
    }

    public async Task DeleteAsync(Guid id)
    {
        var existingContact = await _contactRepository.GetByIdAsync(id);

        if (existingContact is null)
            throw new KeyNotFoundException($"Contact not found. Id: {id}");

        await _contactRepository.DeleteAsync(id);
    }
}