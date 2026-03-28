using PhoneBook.Domain.Enums;

namespace ContactInfoService.Dtos;

public class CreateContactInfoRequest
{
    public Guid ContactId { get; set; }
    public ContactInfoType Type { get; set; }
    public string Content { get; set; } = string.Empty;
}