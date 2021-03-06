using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using pots.Models;

namespace pots.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Step> Steps { get; set; }
        public DbSet<Emotion> Emotions { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserInNotification> UserInNotifications { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Gif> Gifs { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<Step>()
                .HasOne(s => s.User)
                .WithMany(u => u.Step)
                .HasForeignKey(s => s.UserId);
            
            builder.Entity<Emotion>()
                .HasOne(e => e.User)
                .WithMany(u => u.Emotions)
                .HasForeignKey(e => e.UserId);

            builder.Entity<Gif>()
                .HasOne(g => g.Activity)
                .WithMany(a => a.Gifs)
                .HasForeignKey(g => g.ActivityId);
        }
    }
}