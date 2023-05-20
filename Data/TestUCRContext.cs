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

    public virtual DbSet<tb_ACTOR> tb_ACTORs { get; set; }

    public virtual DbSet<tb_AUDIOVISUAL_PRODUCTION> tb_AUDIOVISUAL_PRODUCTIONs { get; set; }

    public virtual DbSet<tb_DIRECTOR> tb_DIRECTORs { get; set; }

    public virtual DbSet<tb_EPISODE> tb_EPISODEs { get; set; }

    public virtual DbSet<tb_GENRE> tb_GENREs { get; set; }

    public virtual DbSet<tb_MOVIE> tb_MOVIEs { get; set; }

    public virtual DbSet<tb_RATING> tb_RATINGs { get; set; }

    public virtual DbSet<tb_SEASON> tb_SEASONs { get; set; }

    public virtual DbSet<tb_SERIE> tb_SERIEs { get; set; }

    public virtual DbSet<tb_USER> tb_USERs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=163.178.173.130;Database=IF4101_2023_VFFN;User ID=basesdedatos; Password=rpbases.2022;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<tb_ACTOR>(entity =>
        {
            entity.HasKey(e => e.ACTOR_ID).HasName("PK__tb_ACTOR__1ED72ACE43291722");

            entity.ToTable("tb_ACTOR");

            entity.Property(e => e.LAST_NAME)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NAME)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<tb_AUDIOVISUAL_PRODUCTION>(entity =>
        {
            entity.HasKey(e => e.TITLE).HasName("PK__tb_AUDIO__475DFD2E135A2A4B");

            entity.ToTable("tb_AUDIOVISUAL_PRODUCTION");

            entity.Property(e => e.TITLE)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CLASSIFICATION)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FRONT_PAGE)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.RELEASE_DATE).HasColumnType("date");
            entity.Property(e => e.SYNOPSIS).HasColumnType("text");
        });

        modelBuilder.Entity<tb_DIRECTOR>(entity =>
        {
            entity.HasKey(e => e.DIRECTOR_ID).HasName("PK__tb_DIREC__71410F6B50C64459");

            entity.ToTable("tb_DIRECTOR");

            entity.Property(e => e.LAST_NAME)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NAME)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<tb_EPISODE>(entity =>
        {
            entity.HasKey(e => e.EPISODE_ID).HasName("PK__tb_EPISO__8960B3EF022A6411");

            entity.ToTable("tb_EPISODE");

            entity.Property(e => e.NAME)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SYNOPSIS).HasColumnType("text");

            entity.HasOne(d => d.SEASON).WithMany(p => p.tb_EPISODEs)
                .HasForeignKey(d => d.SEASON_ID)
                .HasConstraintName("fk_SEASON");
        });

        modelBuilder.Entity<tb_GENRE>(entity =>
        {
            entity.HasKey(e => e.GENRE_NAME).HasName("PK__tb_GENRE__0E30AB4A5A1DBD8E");

            entity.ToTable("tb_GENRE");

            entity.Property(e => e.GENRE_NAME)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.DESCRIPTION).HasColumnType("text");
        });

        modelBuilder.Entity<tb_MOVIE>(entity =>
        {
            entity.HasKey(e => e.TITLE).HasName("PK__tb_MOVIE__475DFD2E9D511A9B");

            entity.ToTable("tb_MOVIE");

            entity.Property(e => e.TITLE)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.TITLENavigation).WithOne(p => p.tb_MOVIE)
                .HasForeignKey<tb_MOVIE>(d => d.TITLE)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_MOVIE_TITLE");
        });

        modelBuilder.Entity<tb_RATING>(entity =>
        {
            entity.HasKey(e => e.RATING_ID).HasName("PK__tb_RATIN__1068AE7233F2F1BC");

            entity.ToTable("tb_RATING");

            entity.HasIndex(e => new { e.TITLE, e.USERNAME }, "UQ__tb_RATIN__AC48433D5F8923ED").IsUnique();

            entity.Property(e => e.COMMENT)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TITLE)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.USERNAME)
                .HasMaxLength(15)
                .IsUnicode(false);

            entity.HasOne(d => d.TITLENavigation).WithMany(p => p.tb_RATINGs)
                .HasForeignKey(d => d.TITLE)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_TITLE");

            entity.HasOne(d => d.USERNAMENavigation).WithMany(p => p.tb_RATINGs)
                .HasForeignKey(d => d.USERNAME)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_USERNAME");
        });

        modelBuilder.Entity<tb_SEASON>(entity =>
        {
            entity.HasKey(e => e.SEASON_ID).HasName("PK__tb_SEASO__CC8E723CE6B4DF67");

            entity.ToTable("tb_SEASON");

            entity.Property(e => e.RELEASE_YEAR)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.TITLE)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.TITLENavigation).WithMany(p => p.tb_SEASONs)
                .HasForeignKey(d => d.TITLE)
                .HasConstraintName("fk_SERIE");
        });

        modelBuilder.Entity<tb_SERIE>(entity =>
        {
            entity.HasKey(e => e.TITLE).HasName("PK__tb_SERIE__475DFD2E99905055");

            entity.ToTable("tb_SERIE");

            entity.Property(e => e.TITLE)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.TITLENavigation).WithOne(p => p.tb_SERIE)
                .HasForeignKey<tb_SERIE>(d => d.TITLE)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_SERIE_TITLE");
        });

        modelBuilder.Entity<tb_USER>(entity =>
        {
            entity.HasKey(e => e.USERNAME).HasName("PK__tb_USER__B15BE12F0DA66012");

            entity.ToTable("tb_USER");

            entity.Property(e => e.USERNAME)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.EMAIL)
                .HasMaxLength(15)
                .IsUnicode(false);
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
