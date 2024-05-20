using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyAssistant.Core.Data.Entities.Auth;

namespace MyAssistant.Core.Data.Context;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        new DbInitializer(modelBuilder).Seed();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        => await base.SaveChangesAsync(cancellationToken);

    //public DbSet<Entity> Actors { get; set; }
}