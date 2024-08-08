using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext : IdentityDbContext<AppUser, AppRole, int,
    IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>,
    IdentityRoleClaim<int>, IdentityUserToken<int>>
{
    public DataContext(DbContextOptions options) : base(options) { }

    public DbSet<Invitation> Invitations { get; set; }

    public DbSet<Message> Messages { get; set; }

    public DbSet<Group> Groups { get; set; }

    public DbSet<Connection> Connections { get; set; }

    public DbSet<Family> Families { get; set; }

    public DbSet<FamilyList> FamilyLists { get; set; }

    public DbSet<AppUserFamily> AppUsersFamilies { get; set; }

    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<AppUser>()
            .HasMany(ur => ur.UserRoles)
            .WithOne(u => u.User)
            .HasForeignKey(u => u.UserId)
            .IsRequired();

        builder.Entity<AppRole>()
            .HasMany(ur => ur.UserRoles)
            .WithOne(u => u.Role)
            .HasForeignKey(u => u.RoleId)
            .IsRequired();

        builder.Entity<AppUserFamily>()
            .HasKey(k => new { k.FamilyId, k.UserId });

        builder.Entity<AppUserFamily>()
            .HasOne(u => u.User)
            .WithMany(u => u.UserFamilies)
            .HasForeignKey(u => u.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<AppUserFamily>()
            .HasOne(u => u.Family)
            .WithMany(u => u.UserFamilies)
            .HasForeignKey(u => u.FamilyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Invitation>()
            .HasIndex(k => new { k.FamilyId, k.InviteeUserId })
            .IsUnique();

        builder.Entity<Invitation>()
            .HasOne(s => s.InviterUser)
            .WithMany(l => l.InvitationsSent)
            .HasForeignKey(s => s.InviterUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Invitation>()
            .HasOne(s => s.InviteeUser)
            .WithMany(l => l.InvitationsReceived)
            .HasForeignKey(s => s.InviteeUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Invitation>()
            .HasOne(s => s.Family)
            .WithMany(l => l.FamilyInvitations)
            .HasForeignKey(s => s.FamilyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Message>()
            .HasOne(u => u.Family)
            .WithMany(m => m.Messages)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Message>()
            .HasOne(u => u.Sender)
            .WithMany(m => m.MessagesSent)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
