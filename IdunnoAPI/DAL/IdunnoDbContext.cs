using IdunnoAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace IdunnoAPI.DAL
{
    public class IdunnoDbContext : DbContext
    {
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public IdunnoDbContext(DbContextOptions<IdunnoDbContext> options) : base(options)
        { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(m => m.ShipperId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(m => m.ReceiverId);
        }
    }
}
