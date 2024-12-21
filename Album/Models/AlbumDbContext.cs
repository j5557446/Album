using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Album.Models
{
    public partial class AlbumDbContext : DbContext
    {
        public AlbumDbContext()
        {
        }

        public AlbumDbContext(DbContextOptions<AlbumDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TAlbum> TAlbums { get; set; } = null!;
        public virtual DbSet<TCategory> TCategories { get; set; } = null!;
        public virtual DbSet<TMember> TMembers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\j5557\\source\\repos\\Album\\Album\\App_Data\\dbAlbum.mdf;Integrated Security=True;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TAlbum>(entity =>
            {
                entity.HasKey(e => e.FAlbumId)
                    .HasName("PK__tAlbum__D284AAB4475B52DD");

                entity.ToTable("tAlbum");

                entity.Property(e => e.FAlbumId).HasColumnName("fAlbumId");

                entity.Property(e => e.FAlbum)
                    .HasMaxLength(50)
                    .HasColumnName("fAlbum");

                entity.Property(e => e.FCid).HasColumnName("fCid");

                entity.Property(e => e.FDescription).HasColumnName("fDescription");

                entity.Property(e => e.FReleaseTime)
                    .HasColumnType("datetime")
                    .HasColumnName("fReleaseTime");

                entity.Property(e => e.FTitle)
                    .HasMaxLength(50)
                    .HasColumnName("fTitle");
            });

            modelBuilder.Entity<TCategory>(entity =>
            {
                entity.HasKey(e => e.FCid)
                    .HasName("PK__tCategor__ADED052C2F66C633");

                entity.ToTable("tCategory");

                entity.Property(e => e.FCid).HasColumnName("fCid");

                entity.Property(e => e.FCname)
                    .HasMaxLength(50)
                    .HasColumnName("fCName");
            });

            modelBuilder.Entity<TMember>(entity =>
            {
                entity.HasKey(e => e.FUid)
                    .HasName("PK__tMember__B791A2ADA66D277F");

                entity.ToTable("tMember");

                entity.Property(e => e.FUid)
                    .HasMaxLength(50)
                    .HasColumnName("fUid");

                entity.Property(e => e.FMail)
                    .HasMaxLength(50)
                    .HasColumnName("fMail");

                entity.Property(e => e.FName)
                    .HasMaxLength(50)
                    .HasColumnName("fName");

                entity.Property(e => e.FPwd)
                    .HasMaxLength(50)
                    .HasColumnName("fPwd");

                entity.Property(e => e.FRole)
                    .HasMaxLength(50)
                    .HasColumnName("fRole");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
