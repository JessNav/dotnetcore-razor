using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Razor.Data.Entities;

namespace Razor;

public partial class RazorAptDbContext : DbContext
{
    public RazorAptDbContext()
    {
    }

    public RazorAptDbContext(DbContextOptions<RazorAptDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Condo> Condos { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=ASUS-G14-NAV\\SQLEXPRESS;Initial Catalog=razorapt;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Comment__3214EC075B8ED250");

            entity.ToTable("Comment");

            entity.Property(e => e.Text).HasMaxLength(255);
            entity.Property(e => e.Username).HasMaxLength(10);

            entity.HasOne(d => d.Condo).WithMany(p => p.Comments)
                .HasForeignKey(d => d.CondoId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Comment_Condo");
        });

        modelBuilder.Entity<Condo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Condo__3214EC07A6448C88");

            entity.ToTable("Condo");

            entity.Property(e => e.Address).HasMaxLength(50);
            entity.Property(e => e.City).HasMaxLength(25);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.State).HasMaxLength(2);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Project__3214EC07201CBF18");

            entity.ToTable("Project");

            entity.HasOne(d => d.Condo).WithMany(p => p.Projects)
                .HasForeignKey(d => d.CondoId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Project_Condo");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
