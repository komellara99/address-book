using Address_Book.Data;
using Address_Book.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class ContactController : ControllerBase
{
    private readonly DataContext _context;

    public ContactController(DataContext context)
    {
        _context = context;
    }

    // [HttpGet]
    // public async Task<ActionResult<IEnumerable<Contact>>> GetContacts(string search)
    // {
    //     
    // }

    [HttpGet]
    public async Task<ActionResult<Contact>> GetAllContacts([FromQuery] string? searchQuery)
    {
        if (string.IsNullOrEmpty(searchQuery))
        {
            return Ok(await _context.Contacts.Include(c => c.Address).ToListAsync());
        }
        
        var filteredContacts = _context.Contacts
            .Where(c => c.FirstName.Contains(searchQuery) || 
                           c.LastName.Contains(searchQuery) || 
                           c.PhoneNumber.Contains(searchQuery)|| 
                           c.Address.Street.Contains(searchQuery) || 
                           c.Address.HouseNo.Contains(searchQuery) || 
                           c.Address.City.Contains(searchQuery) || 
                           c.Address.PostCode.Contains(searchQuery) || 
                           c.Address.Country.Contains(searchQuery))
            .Include(c => c.Address)
            .ToList();

        return Ok(filteredContacts);
       
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Contact>> GetContact(int id)
    {
        var contact = await _context.Contacts
            .Include(contact => contact.Address)
            .FirstOrDefaultAsync(contact => contact.Id == id);

        if (contact == null)
        {
            return NotFound("Contact doesn't exist");
        }
        return contact;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateContact([FromBody] Contact newContact)
    {
        if (await _context.Contacts.AnyAsync(contact => contact.PhoneNumber == newContact.PhoneNumber))
        {
            return BadRequest(new { message = "Contact with this phone number already exists." });
        }
        if (newContact.Address != null)
        {
            _context.Add(newContact.Address);
            await _context.SaveChangesAsync();
            newContact.AddressId = newContact.Address.Id;
        }

        _context.Contacts.Add(newContact);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Contact was created", contact = newContact });
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateContact(int id, Contact updatedContact)
    {
        var contact = await _context.Contacts.FindAsync(id);
        if (contact == null) return NotFound("Contact doesn't exist");

        contact.FirstName = updatedContact.FirstName;
        contact.LastName = updatedContact.LastName;
        contact.PhoneNumber = updatedContact.PhoneNumber;
        contact.Address = updatedContact.Address;

        await _context.SaveChangesAsync();
        return Ok(contact);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContact(int id)
    {
        var contact = await _context.Contacts.Include(c => c.Address).FirstOrDefaultAsync(contact =>contact.Id == id);
        if (contact == null) return NotFound("Contact doesn't exist");

        if (contact.Address != null)
        {
            _context.Addresses.Remove(contact.Address);
        }
        _context.Contacts.Remove(contact);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Contact was deleted." });
    }
    
}