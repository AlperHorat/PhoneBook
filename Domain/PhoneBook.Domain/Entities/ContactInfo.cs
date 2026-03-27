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
                throw new Exception("Content is required");

            ContactId = contactId;
            Type = type;
            Content = content;
        }
    }
}
