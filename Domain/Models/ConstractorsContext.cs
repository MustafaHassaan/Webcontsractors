using System;
using System.Collections.Generic;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Webcontsractors.Domain.Models;

public partial class ConstractorsContext : DbContext
{
    public ConstractorsContext()
    {
    }

    public ConstractorsContext(DbContextOptions<ConstractorsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Opration> Oprations { get; set; }

    public virtual DbSet<Partner> Partners { get; set; }
    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<TblTransaction> TblTransactions { get; set; }

    public virtual DbSet<User> Users { get; set; }
    public DbSet<Menu> Menus { get; set; }
    public DbSet<Submenu> Submenus { get; set; }
    public DbSet<Page> Pages { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<Userpermission> Userpermissions { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Arabic_100_CI_AI");

        modelBuilder.Entity<Opration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Opration__3214EC07905E17C5");

            entity.Property(e => e.Detailes).HasMaxLength(50);
            entity.Property(e => e.Oprationname).HasMaxLength(50);
            entity.Property(e => e.Tblname).HasMaxLength(50);
            entity.Property(e => e.Time).HasMaxLength(50);

            entity.HasOne(d => d.Usr).WithMany(p => p.Oprations)
                .HasForeignKey(d => d.Usrid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Opration_User");
        });

        modelBuilder.Entity<Partner>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Partner");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Partnername).HasMaxLength(250);
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Project");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Amountvat).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Note).HasMaxLength(500);
            entity.Property(e => e.Projectname).HasMaxLength(250);

            entity.HasOne(d => d.Prt).WithMany(p => p.Projects)
                .HasForeignKey(d => d.Prtid)
                .HasConstraintName("FK_Project_Partner");
        });

        modelBuilder.Entity<TblTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Tbl_Transaction");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Creditor).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Debitor).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Detailes).HasMaxLength(500);
            entity.Property(e => e.Tdate).HasColumnName("TDate");
            entity.Property(e => e.Vatamount).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC072B5C2FB7");

            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(150);
        });
        modelBuilder.Entity<Page>()
    .HasOne(p => p.Submenus)
    .WithMany(s => s.Pages)
    .HasForeignKey(p => p.SMId)
    .HasConstraintName("FK_Pages_SubMenus");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
