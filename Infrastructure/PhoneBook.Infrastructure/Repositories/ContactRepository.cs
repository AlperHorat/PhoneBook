using PhoneBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace PhoneBook.Infrastructure.Repositories;

public class ContactRepository : IContactRepository
{
    private readonly ContactDbContext _dbContext;

    public ContactRepository(ContactDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Contact>> GetAllAsync()
    {
        return await _dbContext.Contacts
            .Where(x => !x.IsDeleted)
            .ToListAsync();
    }

    public async Task<Contact?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Contacts
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task AddAsync(Contact contact)
    {
        await _dbContext.Contacts.AddAsync(contact);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var contact = await _dbContext.Contacts.FindAsync(id);
        if (contact != null)
        {
            _dbContext.Contacts.Remove(contact);
            await _dbContext.SaveChangesAsync();
        }
    }
    public async Task SoftDeleteAsync(Guid id)
    {
        var contact = await _dbContext.Contacts.FindAsync(id);

        if (contact is not null)
        {
            contact.SoftDelete();
            await _dbContext.SaveChangesAsync();
        }
    }
}