namespace ContactService.Dtos;

public class ContactDetailResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public List<ContactInfoResponse> ContactInfos { get; set; } = new();
}