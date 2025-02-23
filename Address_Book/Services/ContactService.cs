using Address_Book.Data;
using Address_Book.Models;
using Microsoft.EntityFrameworkCore;

public class ContactService : IContactService
{
    private readonly DataContext _context;

    public ContactService(DataContext context)
    {
        _context = context;
    }

    public async Task<(int totalContacts, List<Contact> contacts)> GetContactsAsync(string? searchQuery, int page)
    {
        var pageSize = 5;
        IQueryable<Contact> query = _context.Contacts.Include(c => c.Address);

        if (!string.IsNullOrEmpty(searchQuery))
        {
            var searchTerms = searchQuery.ToLower().Split(' ');
            query = query.Where(c => searchTerms.All(term =>
                c.FirstName.ToLower().Contains(term) ||
                c.LastName.ToLower().Contains(term) ||
                c.PhoneNumber.ToLower().Contains(term) ||
                (c.Address != null && (
                    c.Address.Street.ToLower().Contains(term) ||
                    c.Address.HouseNo.ToLower().Contains(term) ||
                    c.Address.City.ToLower().Contains(term) ||
                    c.Address.PostCode.ToLower().Contains(term) ||
                    c.Address.Country.ToLower().Contains(term)
                ))
            ));
        }

        var totalContacts = await query.CountAsync();
        var contacts = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return (totalContacts, contacts);
    }

    public async Task<Contact?> GetContactByIdAsync(int id)
    {
        return await _context.Contacts
            .Include(contact => contact.Address)
            .FirstOrDefaultAsync(contact => contact.Id == id);
    }

    public async Task<(bool isSuccess, string message, Contact? contact)> CreateContactAsync(Contact newContact)
    {
        if (await _context.Contacts.AnyAsync(contact => contact.PhoneNumber == newContact.PhoneNumber))
        {
            return (false, "Contact with this phone number already exists.", null);
        }

        if (newContact.Address != null)
        {
            _context.Add(newContact.Address);
            await _context.SaveChangesAsync();
            newContact.AddressId = newContact.Address.Id;
        }

        _context.Contacts.Add(newContact);
        await _context.SaveChangesAsync();

        return (true, "Contact was created", newContact);
    }

    public async Task<(bool isSuccess, string message, Contact? contact)> UpdateContactAsync(int id, Contact updatedContact)
    {
        var contact = await _context.Contacts.FindAsync(id);
        if (contact == null) return (false, "Contact doesn't exist", null);

        if (await _context.Contacts.AnyAsync(c => c.PhoneNumber == updatedContact.PhoneNumber && c.Id != id))
        {
            return (false, "Contact with this phone number already exists.", null);
        }

        contact.FirstName = updatedContact.FirstName;
        contact.LastName = updatedContact.LastName;
        contact.PhoneNumber = updatedContact.PhoneNumber;
        contact.Address = updatedContact.Address;

        await _context.SaveChangesAsync();
        return (true, "Contact updated successfully", contact);
    }

    public async Task<bool> DeleteContactAsync(int id)
    {
        var contact = await _context.Contacts.Include(c => c.Address).FirstOrDefaultAsync(c => c.Id == id);
        if (contact == null) return false;

        if (contact.Address != null)
        {
            _context.Addresses.Remove(contact.Address);
        }
        _context.Contacts.Remove(contact);
        await _context.SaveChangesAsync();
        return true;
    }
}
