using PhoneBook.Domain.Enums;

namespace PhoneBook.Domain.Entities
{
    public class ContactInfo : BaseEntity
    {
        public Guid ContactId { get; private set; }
        public ContactInfoType Type { get; private set; }
        public string Content { get; private set; }

        public ContactInfo(Guid contactId, ContactInfoType type, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Content is required", nameof(content));

            if (type == ContactInfoType.Email && !content.Contains("@"))
                throw new ArgumentException("Invalid email format", nameof(content));

            if (type == ContactInfoType.Phone && content.Length < 10)
                throw new ArgumentException("Invalid phone number", nameof(content));

            ContactId = contactId;
            Type = type;
            Content = content;
        }
    }
}
