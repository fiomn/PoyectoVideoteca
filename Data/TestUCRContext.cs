using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ProyectoVideoteca.Models;

namespace ProyectoVideoteca.Data;

public partial class TestUCRContext : DbContext
{
    public TestUCRContext()
    {
    }

    public TestUCRContext(DbContextOptions<TestUCRContext> options)
        : base(options)
    {
    }

    public virtual DbSet<tb_EPISODE> tb_EPISODE { get; set; }

    public virtual DbSet<tb_GENRE> tb_GENRE { get; set; }

    public virtual DbSet<tb_GLOBALSETTING> tb_GLOBALSETTING { get; set; }

    public virtual DbSet<tb_MOVIE> tb_MOVIE { get; set; }

    public virtual DbSet<tb_RATING> tb_RATING { get; set; }

    public virtual DbSet<tb_SEASON> tb_SEASON { get; set; }

    public virtual DbSet<tb_SECONDARY_ACTOR> tb_SECONDARY_ACTOR { get; set; }

    public virtual DbSet<tb_SECONDARY_GENRE> tb_SECONDARY_GENRE { get; set; }

    public virtual DbSet<tb_SERIE> tb_SERIE { get; set; }

    public virtual DbSet<tb_USER> tb_USER { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=163.178.173.130;Database=IF4101_2023_VFFN;User ID=basesdedatos; Password=rpbases.2022;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<tb_EPISODE>(entity =>
        {
            entity.HasKey(e => e.EPISODE_ID).HasName("PK__tb_EPISO__8960B3EF65341B2A");

            entity.ToTable("tb_EPISODE");

            entity.Property(e => e.DURATION)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.IMG).IsUnicode(false);
            entity.Property(e => e.NAME_EPISODE)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SYNOPSIS).HasColumnType("text");
            entity.Property(e => e.VIDEO).IsUnicode(false);

            entity.HasOne(d => d.SEASON).WithMany(p => p.tb_EPISODEs)
                .HasForeignKey(d => d.SEASON_ID)
                .HasConstraintName("fk_SEASON");
        });

        modelBuilder.Entity<tb_GENRE>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tb_GENRE");

            entity.Property(e => e.GENRE_NAME)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<tb_GLOBALSETTING>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__tb_GLOBA__3213E83FBE77EC70");

            entity.ToTable("tb_GLOBALSETTINGS");

            entity.Property(e => e.mode)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.modeBtn)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        modelBuilder.Entity<tb_MOVIE>(entity =>
        {
            entity.HasKey(e => e.TITLE).HasName("PK__tb_MOVIE__475DFD2E61D839E5");

            entity.ToTable("tb_MOVIE");

            entity.Property(e => e.TITLE)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ACTOR_IMG).IsUnicode(false);
            entity.Property(e => e.ACTOR_NAME).IsUnicode(false);
            entity.Property(e => e.CLASS)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.DIRECTOR_NAME).IsUnicode(false);
            entity.Property(e => e.DURATION)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.GENRE)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.IMG).IsUnicode(false);
            entity.Property(e => e.RELEASE_DATE)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.SYNOPSIS).IsUnicode(false);
            entity.Property(e => e.VIDEO).IsUnicode(false);
        });

        modelBuilder.Entity<tb_RATING>(entity =>
        {
            entity.HasKey(e => e.RATING_ID).HasName("PK__tb_RATIN__1068AE721669667F");

            entity.ToTable("tb_RATING");

            entity.HasIndex(e => new { e.TITLE, e.USERNAME }, "UQ__tb_RATIN__AC48433D41FD748D").IsUnique();

            entity.Property(e => e.COMMENT)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TITLE)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.USERNAME)
                .HasMaxLength(15)
                .IsUnicode(false);

            entity.HasOne(d => d.USERNAMENavigation).WithMany(p => p.tb_RATINGs)
                .HasForeignKey(d => d.USERNAME)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_USERNAME");
        });

        modelBuilder.Entity<tb_SEASON>(entity =>
        {
            entity.HasKey(e => e.SEASON_ID).HasName("PK__tb_SEASO__CC8E723CB64A2445");

            entity.ToTable("tb_SEASON");

            entity.Property(e => e.TITLE)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.VIDEO).IsUnicode(false);

            entity.HasOne(d => d.TITLENavigation).WithMany(p => p.tb_SEASONs)
                .HasForeignKey(d => d.TITLE)
                .HasConstraintName("fk_SERIE");
        });

        modelBuilder.Entity<tb_SECONDARY_ACTOR>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tb_SECONDARY_ACTOR");

            entity.Property(e => e.ACTOR_IMG).IsUnicode(false);
            entity.Property(e => e.ACTOR_NAME).IsUnicode(false);
            entity.Property(e => e.MOVIE_TITLE)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.SERIE_TITLE)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.MOVIE_TITLENavigation).WithMany()
                .HasForeignKey(d => d.MOVIE_TITLE)
                .HasConstraintName("fk_Movie_Id");

            entity.HasOne(d => d.SERIE_TITLENavigation).WithMany()
                .HasForeignKey(d => d.SERIE_TITLE)
                .HasConstraintName("fk_SERIE_TITLE");
        });

        modelBuilder.Entity<tb_SECONDARY_GENRE>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tb_SECONDARY_GENRE");

            entity.Property(e => e.GENRE_NAME)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.MOVIE_TITLE)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.MOVIE_TITLENavigation).WithMany()
                .HasForeignKey(d => d.MOVIE_TITLE)
                .HasConstraintName("fk_Movie_Iitle");
        });

        modelBuilder.Entity<tb_SERIE>(entity =>
        {
            entity.HasKey(e => e.TITLE).HasName("PK__tb_SERIE__475DFD2E5F9AB7CB");

            entity.ToTable("tb_SERIE");

            entity.Property(e => e.TITLE)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ACTOR_IMG).IsUnicode(false);
            entity.Property(e => e.ACTOR_NAME).IsUnicode(false);
            entity.Property(e => e.CLASS)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.DIRECTOR_NAME).IsUnicode(false);
            entity.Property(e => e.GENRE)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.IMG).IsUnicode(false);
            entity.Property(e => e.RELEASE_DATE)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.SYNOPSIS).IsUnicode(false);
            entity.Property(e => e.VIDEO).IsUnicode(false);
        });

        modelBuilder.Entity<tb_USER>(entity =>
        {
            entity.HasKey(e => e.USERNAME).HasName("PK__tb_USER__B15BE12FC8681285");

            entity.ToTable("tb_USER");

            entity.Property(e => e.USERNAME)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.EMAIL)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IMG).IsUnicode(false);
            entity.Property(e => e.NAME)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PASSWORD)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.PASSWORD_CONFIRM)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.ROLE)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
