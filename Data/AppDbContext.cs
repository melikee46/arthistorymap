using Microsoft.EntityFrameworkCore;
using ArtHistoryMap.Api.Entities;

namespace ArtHistoryMap.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ArtMovement> ArtMovements => Set<ArtMovement>();
        public DbSet<ArtMovementRelation> ArtMovementRelations => Set<ArtMovementRelation>();
        public DbSet<Artist> Artists => Set<Artist>();
        public DbSet<ArtistMovement> ArtistMovements => Set<ArtistMovement>();
        public DbSet<Artwork> Artworks => Set<Artwork>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---------------------------------------------------------
            // ArtMovementRelation: self-referencing, YÖNLÜ ilişki
            // İki ayrı FK, aynı tabloya (ArtMovement) işaret ediyor.
            // EF Core bunu convention ile çözemez, Fluent API ile
            // elle, açıkça tanımlamamız ZORUNLU.
            // ---------------------------------------------------------
            modelBuilder.Entity<ArtMovementRelation>()
                .HasOne(r => r.SourceMovement)
                .WithMany(m => m.OutgoingRelations)
                .HasForeignKey(r => r.SourceMovementId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ArtMovementRelation>()
                .HasOne(r => r.TargetMovement)
                .WithMany(m => m.IncomingRelations)
                .HasForeignKey(r => r.TargetMovementId)
                .OnDelete(DeleteBehavior.Restrict);

            // Aynı kaynak-hedef ikilisi birden fazla RelationType ile
            // tekrar edebilir (Soru 4: Barok->Rokoko hem ContemporaryWith
            // hem ReactionTo olabilir), bu yüzden unique constraint
            // ÜÇ kolon üzerinde: tekrarını engelliyoruz ama
// RelationType'a izin veriyoruz.
            modelBuilder.Entity<ArtMovementRelation>()
                .HasIndex(r => new { r.SourceMovementId, r.TargetMovementId, r.RelationType })
                .IsUnique();

            // ---------------------------------------------------------
            // ArtistMovement: many-to-many ara tablo (composite key)
            // ---------------------------------------------------------
            modelBuilder.Entity<ArtistMovement>()
                .HasKey(am => new { am.ArtistId, am.MovementId });

            modelBuilder.Entity<ArtistMovement>()
                .HasOne(am => am.Artist)
                .WithMany(a => a.ArtistMovements)
                .HasForeignKey(am => am.ArtistId);

            modelBuilder.Entity<ArtistMovement>()
                .HasOne(am => am.Movement)
                .WithMany(m => m.ArtistMovements)
                .HasForeignKey(am => am.MovementId);
        }
    }
}
