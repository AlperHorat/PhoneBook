using PhoneBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace PhoneBook.Infrastructure.Repositories;

public class ContactRepository : IContactRepository
{
    private readonly AppDbContext _db;

    public ContactRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Contact>> GetAllAsync()
    {
        return await _db.Contacts.ToListAsync();
    }

    public async Task<Contact?> GetByIdAsync(Guid id)
    {
        return await _db.Contacts.FindAsync(id);
    }

    public async Task AddAsync(Contact contact)
    {
        await _db.Contacts.AddAsync(contact);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var contact = await _db.Contacts.FindAsync(id);
        if (contact != null)
        {
            _db.Contacts.Remove(contact);
            await _db.SaveChangesAsync();
        }
    }
}