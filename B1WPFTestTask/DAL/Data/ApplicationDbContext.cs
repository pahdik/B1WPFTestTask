using B1WPFTestTask.DAL.Entities;
using B1WPFTestTask.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace B1WPFTestTask.DAL.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Balance> Balances { get; set; }
    public DbSet<AccountClass> AccountClasses { get; set; }
    public DbSet<Bank> Banks { get; set; }
    public DbSet<FileInformation> FileInformatinos { get; set; }
    public DbSet<AccountGroup> AccountGroups { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        //Database.EnsureDeleted();
        Database.EnsureCreated();
    }
}
