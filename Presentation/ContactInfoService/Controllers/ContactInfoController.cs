using ContactInfoService.Dtos;
using Microsoft.AspNetCore.Mvc;
using PhoneBook.Domain.Entities;
using PhoneBook.Services.ContactInfos;

namespace ContactInfoService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactInfoController : ControllerBase
{
    private readonly IContactInfoService _contactInfoService;

    public ContactInfoController(IContactInfoService contactInfoService)
    {
        _contactInfoService = contactInfoService;
    }

    [HttpGet("contact/{contactId:guid}")]
    public async Task<IActionResult> GetByContactId(Guid contactId)
    {
        var contactInfos = await _contactInfoService.GetByContactIdAsync(contactId);
        return Ok(contactInfos);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateContactInfoRequest request)
    {
        var entity = new ContactInfo(request.ContactId, request.Type, request.Content);

        var created = await _contactInfoService.CreateAsync(entity);

        return Ok(created);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _contactInfoService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPatch("{id:guid}/soft-delete")]
    public async Task<IActionResult> SoftDelete(Guid id)
    {
        await _contactInfoService.SoftDeleteAsync(id);
        return NoContent();
    }
}