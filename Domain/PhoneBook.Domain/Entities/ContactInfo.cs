using PhoneBook.Domain.Enums;

namespace PhoneBook.Domain.Entities
{
    public class ContactInfo
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ContactId { get; set; }
        public ContactInfoType Type { get; set; }
        public string Content { get; set; }
    }
}
