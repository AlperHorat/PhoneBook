using Microsoft.EntityFrameworkCore;
using PhoneBook.Domain.Entities;

namespace PhoneBook.Infrastructure.Data;

public class ContactInfoDbContext : DbContext
{
    public ContactInfoDbContext(DbContextOptions<ContactInfoDbContext> options) : base(options)
    {
    }

    public DbSet<ContactInfo> ContactInfos { get; set; }
}