
namespace PhoneBook.Domain.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();
        public DateTime CreatedDate { get; protected set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; protected set; }
        public bool IsDeleted { get; protected set; } = false;

        public void SetUpdated()
        {
            UpdatedDate = DateTime.UtcNow;
        }

        public void SoftDelete()
        {
            IsDeleted = true;
            SetUpdated();
        }
    }
}
