using Address_Book.Models;

public interface IContactService
{
    Task<(int totalContacts, List<Contact> contacts)> GetContactsAsync(string? searchQuery, int page);
    Task<Contact?> GetContactByIdAsync(int id);
    Task<(bool isSuccess, string message, Contact? contact)> CreateContactAsync(Contact newContact);
    Task<(bool isSuccess, string message, Contact? contact)> UpdateContactAsync(int id, Contact updatedContact);
    Task<bool> DeleteContactAsync(int id);
}