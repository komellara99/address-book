using Address_Book.Models;
using Microsoft.EntityFrameworkCore;

namespace Address_Book.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) {}

    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Address> Addresses { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contact>()
            .HasOne(c => c.Address) 
            .WithMany()
            .HasForeignKey(c => c.AddressId)
            .OnDelete(DeleteBehavior.Cascade); 
    }
}