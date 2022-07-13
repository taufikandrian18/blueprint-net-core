using System;
using Microsoft.EntityFrameworkCore;

namespace WITNetCoreProject.Models.Entities {

    public class RepositoryContext : DbContext {

        public RepositoryContext(DbContextOptions options) : base(options) {
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<RefreshTokens> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder.Entity<Users>(entity => {

                entity.Property(e => e.Username).IsUnicode(false);
                entity.Property(e => e.Password).IsUnicode(false);
                entity.Property(e => e.DisplayName).IsUnicode(false);
                entity.Property(e => e.Phone).IsUnicode(false);
                entity.Property(e => e.Email).IsUnicode(false);

                //entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");

                entity.Property(e => e.CreatedBy)
                            .IsUnicode(false)
                            .HasDefaultValueSql("('SYSTEM')");

                //entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

                entity.Property(e => e.UpdatedBy)
                            .IsUnicode(false)
                            .HasDefaultValueSql("('SYSTEM')");

                //entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
                entity.Property(e => e.IsDeleted).HasDefaultValue(false); ;

            });

            modelBuilder.Entity<RefreshTokens>(entity => {

                //entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.Token).IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.RefreshTokens)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RefreshTokens_Users");

            });
        }
    }
}
