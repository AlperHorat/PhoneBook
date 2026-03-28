using PhoneBook.Domain.Enums;

namespace ContactService.Dtos;

public class ContactInfoResponse
{
    public Guid Id { get; set; }
    public Guid ContactId { get; set; }
    public ContactInfoType Type { get; set; }
    public string Content { get; set; } = string.Empty;
}