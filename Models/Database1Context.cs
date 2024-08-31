using System;
using System.Collections.Generic;
using ProductSearch.Models;
using Microsoft.EntityFrameworkCore;

namespace ProductSearch.Models;

public partial class Database1Context : DbContext
{
    public Database1Context()
    {
    }

    public Database1Context(DbContextOptions<Database1Context> options)
        : base(options)
    {
    }

    public DbSet<Admin> Admins { get; set; }
    public DbSet<Supervisor> Supervisors { get; set; }
    public DbSet<Cosupervisor> CoSupervisors { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Keyword> Keywords { get; set; }
    public DbSet<Person> People { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=EMRECAN;Database=Database1;Trusted_Connection=True;TrustServerCertificate=True").LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)
        .EnableSensitiveDataLogging();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>()
            .HasDiscriminator<string>("PersonType")
            .HasValue<Supervisor>("Supervisor")
            .HasValue<Cosupervisor>("CoSupervisor");

        modelBuilder.Entity<Supervisor>()
            .Property(s => s.Department).HasColumnName("Department");

        modelBuilder.Entity<Cosupervisor>()
            .Property(c => c.Department).HasColumnName("Department");

        modelBuilder.Entity<Product>()
             .HasMany(p => p.Keywords)
             .WithOne(k => k.Product)
             .HasForeignKey(k => k.ProductId);

        modelBuilder.Entity<Product>()
             .HasOne(p => p.Supervisor)
             .WithMany()
             .HasForeignKey(p => p.SupervisorId)
             .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Product>()
             .HasOne(p => p.CoSupervisor)
             .WithMany()
             .HasForeignKey(p => p.CoSupervisorId)
             .OnDelete(DeleteBehavior.Restrict);
        

        modelBuilder.Entity<Keyword>()
             .HasOne(p => p.Product)
             .WithMany(k => k.Keywords)
             .HasForeignKey(k => k.ProductId)
             .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Product>()
              .HasOne(p => p.CreatedBy)
              .WithMany()
              .HasForeignKey(p => p.CreatedById)
               .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Product>()
            .HasOne(p => p.UpdatedBy)
            .WithMany()
            .HasForeignKey(p => p.UpdatedById)
            .OnDelete(DeleteBehavior.Restrict);
    }

}

