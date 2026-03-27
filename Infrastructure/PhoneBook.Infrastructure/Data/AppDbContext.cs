using Microsoft.EntityFrameworkCore;
using PhoneBook.Domain.Entities;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Contact> Contacts { get; set; }
    public DbSet<ContactInfo> ContactInfos { get; set; }
}