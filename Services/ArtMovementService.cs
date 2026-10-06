using Microsoft.EntityFrameworkCore;
using ArtHistoryMap.Api.Data;
using ArtHistoryMap.Api.DTOs;

namespace ArtHistoryMap.Api.Services
{
    public class ArtMovementService
    {
        private readonly AppDbContext _context;

        public ArtMovementService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ArtMovementSummaryDto>> GetAllAsync()
        {
            return await _context.ArtMovements
                .Select(m => new ArtMovementSummaryDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    StartYear = m.StartYear,
                    EndYear = m.EndYear,
                    Region = m.Region
                })
                .ToListAsync();
        }

        public async Task<ArtMovementDetailDto?> GetByIdAsync(Guid id)
        {
            var movement = await _context.ArtMovements
                .Include(m => m.OutgoingRelations)
                    .ThenInclude(r => r.TargetMovement)
                .Include(m => m.IncomingRelations)
                    .ThenInclude(r => r.SourceMovement)
                .Include(m => m.ArtistMovements)
                    .ThenInclude(am => am.Artist)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movement == null) return null;

            return new ArtMovementDetailDto
            {
                Id = movement.Id,
                Name = movement.Name,
                StartYear = movement.StartYear,
                EndYear = movement.EndYear,
                Description = movement.Description,
                Region = movement.Region,
                OutgoingRelations = movement.OutgoingRelations.Select(r => new RelationDto
                {
                    RelatedMovementId = r.TargetMovementId,
                    RelatedMovementName = r.TargetMovement.Name,
                    RelationType = r.RelationType.ToString(),
                    Description = r.Description
                }).ToList(),
                IncomingRelations = movement.IncomingRelations.Select(r => new RelationDto
                {
                    RelatedMovementId = r.SourceMovementId,
                    RelatedMovementName = r.SourceMovement.Name,
                    RelationType = r.RelationType.ToString(),
                    Description = r.Description
                }).ToList(),
                ArtistNames = movement.ArtistMovements.Select(am => am.Artist.Name).ToList()
            };
        }
    }
}
