using PhoneBook.Domain.Entities;
using PhoneBook.Infrastructure.Repositories;

namespace PhoneBook.Services.ContactInfos;

public class ContactInfoManager : IContactInfoService
{
    private readonly IContactInfoRepository _contactInfoRepository;

    public ContactInfoManager(IContactInfoRepository contactInfoRepository)
    {
        _contactInfoRepository = contactInfoRepository;
    }

    public async Task<List<ContactInfo>> GetByContactIdAsync(Guid contactId)
    {
        return await _contactInfoRepository.GetByContactIdAsync(contactId);
    }

    public async Task<ContactInfo> CreateAsync(ContactInfo contactInfo)
    {
        if (contactInfo is null)
            throw new ArgumentNullException(nameof(contactInfo));

        await _contactInfoRepository.AddAsync(contactInfo);
        return contactInfo;
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _contactInfoRepository.GetByIdAsync(id);

        if (entity is null)
            throw new KeyNotFoundException($"Contact info not found. Id: {id}");

        await _contactInfoRepository.DeleteAsync(id);
    }

    public async Task SoftDeleteAsync(Guid id)
    {
        var entity = await _contactInfoRepository.GetByIdAsync(id);

        if (entity is null)
            throw new KeyNotFoundException($"Contact info not found. Id: {id}");

        await _contactInfoRepository.SoftDeleteAsync(id);
    }
    public async Task SoftDeleteByContactIdAsync(Guid contactId)
    {
        await _contactInfoRepository.SoftDeleteByContactIdAsync(contactId);
    }
}