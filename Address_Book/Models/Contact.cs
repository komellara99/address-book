
using System.ComponentModel.DataAnnotations;

namespace Address_Book.Models;

public class Contact
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "First name is required.")]
    public string FirstName { get; set; }
    
    [Required(ErrorMessage = "Last name is required.")]
    public string LastName { get; set; }
    
    [Required(ErrorMessage = "Phone number is required.")]
    [RegularExpression(@"^\+?[0-9]{7,15}$", ErrorMessage = "Invalid phone number.")]
    public string PhoneNumber { get; set; }
    
    public int AddressId { get; set; }
    public Address Address { get; set; }
}
