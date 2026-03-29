using Microsoft.EntityFrameworkCore;
using PhoneBook.Domain.Entities;
using PhoneBook.Infrastructure.Data;

namespace PhoneBook.Infrastructure.Repositories;

public class ContactInfoRepository : IContactInfoRepository
{
    private readonly ContactInfoDbContext _dbContext;

    public ContactInfoRepository(ContactInfoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ContactInfo>> GetByContactIdAsync(Guid contactId)
    {
        return await _dbContext.ContactInfos
            .Where(x => x.ContactId == contactId && !x.IsDeleted)
            .ToListAsync();
    }

    public async Task<ContactInfo?> GetByIdAsync(Guid id)
    {
        return await _dbContext.ContactInfos
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task AddAsync(ContactInfo contactInfo)
    {
        await _dbContext.ContactInfos.AddAsync(contactInfo);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _dbContext.ContactInfos.FindAsync(id);

        if (entity is not null)
        {
            _dbContext.ContactInfos.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task SoftDeleteAsync(Guid id)
    {
        var entity = await _dbContext.ContactInfos.FindAsync(id);

        if (entity is not null)
        {
            entity.SoftDelete();
            await _dbContext.SaveChangesAsync();
        }
    }
    public async Task SoftDeleteByContactIdAsync(Guid contactId)
    {
        var entities = await _dbContext.ContactInfos
            .Where(x => x.ContactId == contactId && !x.IsDeleted)
            .ToListAsync();

        foreach (var entity in entities)
        {
            entity.SoftDelete();
        }

        await _dbContext.SaveChangesAsync();
    }
}