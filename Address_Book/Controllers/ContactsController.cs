using Address_Book.Data;
using Address_Book.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class ContactController : ControllerBase
{
    private readonly IContactService _contactService;

    public ContactController(IContactService contactService)
    {
        _contactService = contactService;
    }

    [HttpGet]
    public async Task<ActionResult<Contact>> GetContactsAsync([FromQuery] string? searchQuery, [FromQuery] int page = 1)
    {
        var (totalContacts, contacts) = await _contactService.GetContactsAsync(searchQuery, page);
        return Ok(new
        {
            TotalContacts = totalContacts,
            Contacts = contacts
        });
       
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Contact>> GetContactAsync(int id)
    {
        var contact = await _contactService.GetContactByIdAsync(id);
        if (contact == null) return NotFound("Contact doesn't exist");
        return Ok(contact);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateContactAsync([FromBody] Contact newContact)
    {
        var result = await _contactService.CreateContactAsync(newContact);
        if (!result.isSuccess) return BadRequest(new { message = result.message });

        return Ok(new { message = "Contact was created", contact = result.contact });
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateContactAsync(int id, Contact updatedContact)
    {
        var result = await _contactService.UpdateContactAsync(id, updatedContact);
        if (!result.isSuccess) return BadRequest(new { message = result.message });

        return Ok(new { message = result.message, contact = result.contact });
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContactAsync(int id)
    {
        var isDeleted = await _contactService.DeleteContactAsync(id);
        if (!isDeleted) return NotFound("Contact doesn't exist");

        return Ok(new { message = "Contact was deleted." });
    }
    
}