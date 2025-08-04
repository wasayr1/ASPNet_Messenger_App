using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SignalRMessenger.Models;

namespace SignalRMessenger.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
    base.OnModelCreating(builder); 

    builder.Entity<Message>()
        .HasOne(m => m.Sender)
        .WithMany()
        .HasForeignKey(m => m.SenderId)
        .OnDelete(DeleteBehavior.Restrict); // Change cascade delete to restrict

    builder.Entity<Message>()
        .HasOne(m => m.Receiver)
        .WithMany()
        .HasForeignKey(m => m.ReceiverId)
        .OnDelete(DeleteBehavior.Restrict); // Change cascade delete to restrict
    }   
}

