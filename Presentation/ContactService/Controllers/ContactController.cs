using ContactService.Clients;
using ContactService.Dtos;
using Microsoft.AspNetCore.Mvc;
using PhoneBook.Domain.Entities;
using PhoneBook.Services.Contacts;

namespace ContactService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactController : ControllerBase
{
    private readonly IContactService _contactService;
    private readonly IContactInfoApiClient _contactInfoApiClient;

    public ContactController(
        IContactService contactService,
        IContactInfoApiClient contactInfoApiClient)
    {
        _contactService = contactService;
        _contactInfoApiClient = contactInfoApiClient;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var contacts = await _contactService.GetAllAsync();
        return Ok(contacts);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var contact = await _contactService.GetByIdAsync(id);

        if (contact is null)
            return NotFound();

        return Ok(contact);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateContactRequest request)
    {
        var contact = new Contact(request.Name, request.Surname, request.Company);

        var createdContact = await _contactService.CreateAsync(contact);

        return CreatedAtAction(nameof(GetById), new { id = createdContact.Id }, createdContact);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _contactService.DeleteAsync(id);
        return NoContent();
    }
    [HttpPatch("{id:guid}/soft-delete")]
    public async Task<IActionResult> SoftDelete(Guid id)
    {
        await _contactService.SoftDeleteAsync(id);
        return NoContent();
    }

    [HttpGet("{id:guid}/detail")]
    public async Task<IActionResult> GetDetail(Guid id)
    {
        var contact = await _contactService.GetByIdAsync(id);

        if (contact is null)
            return NotFound();

        var contactInfos = await _contactInfoApiClient.GetByContactIdAsync(id);

        var response = new ContactDetailResponse
        {
            Id = contact.Id,
            Name = contact.Name,
            Surname = contact.Surname,
            Company = contact.Company,
            ContactInfos = contactInfos
        };

        return Ok(response);
    }
}