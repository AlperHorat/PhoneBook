using PhoneBook.Domain.Entities;

namespace PhoneBook.Infrastructure.Repositories
{
    public interface IContactRepository
    {
        Task<List<Contact>> GetAllAsync();
        Task<Contact?> GetByIdAsync(Guid id);
        Task AddAsync(Contact contact);
        Task DeleteAsync(Guid id);
        Task SoftDeleteAsync(Guid id);
    }
}
