using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CFProject_T6.Models
{
    public partial class ProjectContext : DbContext
    {
        public ProjectContext()
        {
        }

        public ProjectContext(DbContextOptions<ProjectContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BackersProjects> BackersProjects { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Packages> Packages { get; set; }
        public virtual DbSet<Photos> Photos { get; set; }
        public virtual DbSet<Projects> Projects { get; set; }
        public virtual DbSet<Updates> Updates { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Videos> Videos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BackersProjects>(entity =>
            {
                entity.ToTable("Backers_projects");

                entity.Property(e => e.PackageId).HasColumnName("Package_Id");

                entity.Property(e => e.ProjectId).HasColumnName("Project_Id");

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.HasOne(d => d.Package)
                    .WithMany(p => p.BackersProjects)
                    .HasForeignKey(d => d.PackageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Backers_projects_Packages");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.BackersProjects)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Backers_projects_Projects");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BackersProjects)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Backers_projects_Users");
            });

            modelBuilder.Entity<Categories>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Packages>(entity =>
            {
                entity.Property(e => e.DonationUpperlim)
                    .HasColumnName("Donation_upperlim")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.ProjectId).HasColumnName("Project_Id");

                entity.Property(e => e.Reward)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Packages)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Packages_Projects");
            });

            modelBuilder.Entity<Photos>(entity =>
            {
                entity.Property(e => e.Filename)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProjectId).HasColumnName("Project_Id");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Photos)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Photos_Projects");
            });

            modelBuilder.Entity<Projects>(entity =>
            {
                entity.Property(e => e.CategoryId).HasColumnName("Category_Id");

                entity.Property(e => e.CreatorId).HasColumnName("Creator_Id");

                entity.Property(e => e.Descr)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.EndDate).HasColumnType("smalldatetime");

                entity.Property(e => e.Fundsrecv).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Goalfunds).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.StartDate).HasColumnType("smalldatetime");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Projects_Categories");

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.CreatorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_projects_users");
            });

            modelBuilder.Entity<Updates>(entity =>
            {
                entity.Property(e => e.Descr)
                    .IsRequired()
                    .HasColumnType("ntext");

                entity.Property(e => e.ProjectId).HasColumnName("Project_Id");

                entity.Property(e => e.Timestamp).HasColumnType("smalldatetime");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Updates)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Updates_Projects");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(320)
                    .IsUnicode(false);

                entity.Property(e => e.Fname)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Lname)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(180)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Videos>(entity =>
            {
                entity.Property(e => e.Filename)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProjectId).HasColumnName("Project_Id");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Videos)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Videos_Projects");
            });
        }
    }
}
