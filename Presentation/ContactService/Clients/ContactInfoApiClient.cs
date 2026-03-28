using System.Net.Http.Json;
using ContactService.Dtos;

namespace ContactService.Clients;

public class ContactInfoApiClient : IContactInfoApiClient
{
    private readonly HttpClient _httpClient;

    public ContactInfoApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ContactInfoResponse>> GetByContactIdAsync(Guid contactId)
    {
        var response = await _httpClient.GetFromJsonAsync<List<ContactInfoResponse>>(
            $"api/contactinfo/contact/{contactId}");

        return response ?? new List<ContactInfoResponse>();
    }
}