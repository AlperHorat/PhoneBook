namespace ContactService.Dtos;

public class CreateContactRequest
{
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
}