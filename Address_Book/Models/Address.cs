using System.ComponentModel.DataAnnotations;

namespace Address_Book.Models;

public class Address
{
    public int Id { get; set; } 
    
    [Required(ErrorMessage = "Street is required.")]
    public string Street { get; set; }
    
    [Required(ErrorMessage = "House number is required.")]
    public string HouseNo { get; set; }
    
    [Required(ErrorMessage = "City is required.")]
    public string City { get; set; }
    
    [Required(ErrorMessage = "Post code is required.")]
    public string PostCode { get; set; }
    
    [Required(ErrorMessage = "Country is required.")]
    public string Country { get; set; }
    
}