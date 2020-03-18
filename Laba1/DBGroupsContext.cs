using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Laba1
{
    public partial class DBGroupsContext : DbContext
    {
        public DBGroupsContext()
        {
        }

        public DBGroupsContext(DbContextOptions<DBGroupsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Albums> Albums { get; set; }
        public virtual DbSet<AlbumsSongs> AlbumsSongs { get; set; }
        public virtual DbSet<Artists> Artists { get; set; }
        public virtual DbSet<Countries> Countries { get; set; }
        public virtual DbSet<Genres> Genres { get; set; }
        public virtual DbSet<Groups> Groups { get; set; }
        public virtual DbSet<GroupsAlbums> GroupsAlbums { get; set; }
        public virtual DbSet<Songs> Songs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //To protect potentially sensitive information in your connection string, you should move it out of source code. 
                //See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=OLYA\\SQLEXPRESS;Database=DBGroups;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Albums>(entity =>
            {
                entity.HasKey(e => e.AId);

                entity.Property(e => e.AId).HasColumnName("A_Id");

                entity.Property(e => e.ACreation)
                    .HasColumnName("A_Creation")
                    .HasColumnType("date");

                entity.Property(e => e.AName)
                    .IsRequired()
                    .HasColumnName("A_Name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<AlbumsSongs>(entity =>
            {
                entity.ToTable("Albums_Songs");

                entity.Property(e => e.AlbumId).HasColumnName("Album_Id");

                entity.Property(e => e.SongId).HasColumnName("Song_Id");
            });

            modelBuilder.Entity<Artists>(entity =>
            {
                entity.HasKey(e => e.AId);

                entity.Property(e => e.AId).HasColumnName("A_Id");

                entity.Property(e => e.ABirth)
                    .HasColumnName("A_Birth")
                    .HasColumnType("date");

                entity.Property(e => e.AGender).HasColumnName("A_Gender");

                entity.Property(e => e.AName)
                    .IsRequired()
                    .HasColumnName("A_Name")
                    .HasMaxLength(50);

                entity.Property(e => e.APhone)
                    .HasColumnName("A_Phone")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CountryId).HasColumnName("Country_Id");

                entity.Property(e => e.GroupId).HasColumnName("Group_Id");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Artists)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Artists_Countries");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Artists)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Artists_Groups");
            });

            modelBuilder.Entity<Countries>(entity =>
            {
                entity.HasKey(e => e.CId);

                entity.Property(e => e.CId).HasColumnName("C_Id");

                entity.Property(e => e.CCapital)
                    .IsRequired()
                    .HasColumnName("C_Capital")
                    .HasMaxLength(50);

                entity.Property(e => e.CLang)
                    .IsRequired()
                    .HasColumnName("C_Lang")
                    .HasMaxLength(50);

                entity.Property(e => e.CName)
                    .IsRequired()
                    .HasColumnName("C_Name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Genres>(entity =>
            {
                entity.HasKey(e => e.GenId);

                entity.Property(e => e.GenId).HasColumnName("Gen_Id");

                entity.Property(e => e.GenName)
                    .IsRequired()
                    .HasColumnName("Gen_Name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Groups>(entity =>
            {
                entity.HasKey(e => e.GrId);

                entity.Property(e => e.GrId).HasColumnName("Gr_Id");

                entity.Property(e => e.GrCreation)
                    .HasColumnName("Gr_Creation")
                    .HasColumnType("date");

                entity.Property(e => e.GrName)
                    .IsRequired()
                    .HasColumnName("Gr_Name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<GroupsAlbums>(entity =>
            {
                entity.ToTable("Groups_Albums");

                entity.Property(e => e.AlbumId).HasColumnName("Album_Id");

                entity.Property(e => e.GroupId).HasColumnName("Group_Id");

                entity.Property(e => e.Info).HasColumnType("ntext");

                entity.HasOne(d => d.Album)
                    .WithMany(p => p.GroupsAlbums)
                    .HasForeignKey(d => d.AlbumId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Groups_Albums_Albums");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupsAlbums)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Groups_Albums_Groups");
            });

            modelBuilder.Entity<Songs>(entity =>
            {
                entity.HasKey(e => e.SId);

                entity.Property(e => e.SId).HasColumnName("S_Id");

                entity.Property(e => e.AlbumId).HasColumnName("Album_Id");

                entity.Property(e => e.GenreId).HasColumnName("Genre_Id");

                entity.Property(e => e.SDuration).HasColumnName("S_Duration");

                entity.Property(e => e.SName)
                    .IsRequired()
                    .HasColumnName("S_Name")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Album)
                    .WithMany(p => p.Songs)
                    .HasForeignKey(d => d.AlbumId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Songs_Albums");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.Songs)
                    .HasForeignKey(d => d.GenreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Songs_Genres");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
