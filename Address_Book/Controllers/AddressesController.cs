using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Address_Book.Data;

[Route("api/[controller]")]
[ApiController]
public class AddressController : ControllerBase
{
    private readonly DataContext _context;

    public AddressController(DataContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAddresses()
    {
        return Ok(await _context.Addresses.ToListAsync());
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAddress(int id)
    {
        var address = await _context.Addresses.FindAsync(id);
        if (address == null) return NotFound("Address does not exist");

        return Ok(address);
    }
}