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
                .AsNoTracking()
                .OrderBy(m => m.StartYear)
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
            return await _context.ArtMovements
                .AsNoTracking()
                .AsSplitQuery()
                .Where(m => m.Id == id)
                .Select(m => new ArtMovementDetailDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    StartYear = m.StartYear,
                    EndYear = m.EndYear,
                    Description = m.Description,
                    Region = m.Region,
                    OutgoingRelations = m.OutgoingRelations.Select(r => new RelationDto
                    {
                        RelatedMovementId = r.TargetMovementId,
                        RelatedMovementName = r.TargetMovement.Name,
                        RelationType = r.RelationType.ToString(),
                        Description = r.Description
                    }).ToList(),
                    IncomingRelations = m.IncomingRelations.Select(r => new RelationDto
                    {
                        RelatedMovementId = r.SourceMovementId,
                        RelatedMovementName = r.SourceMovement.Name,
                        RelationType = r.RelationType.ToString(),
                        Description = r.Description
                    }).ToList(),
                    ArtistNames = m.ArtistMovements.Select(am => am.Artist.Name).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<GraphDto> GetGraphAsync()
        {
            var nodes = await _context.ArtMovements
                .AsNoTracking()
                .OrderBy(m => m.StartYear)
                .Select(m => new GraphNodeDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    StartYear = m.StartYear,
                    EndYear = m.EndYear,
                    Region = m.Region
                })
                .ToListAsync();

            // Ham veriyi çek, enum -> string çevirisini bellekte yap
            var rawLinks = await _context.ArtMovementRelations
                .AsNoTracking()
                .Select(r => new { r.SourceMovementId, r.TargetMovementId, r.RelationType, r.Description })
                .ToListAsync();

            var links = rawLinks.Select(r => new GraphLinkDto
            {
                Source = r.SourceMovementId,
                Target = r.TargetMovementId,
                RelationType = r.RelationType.ToString(),
                Description = r.Description
            }).ToList();

            return new GraphDto { Nodes = nodes, Links = links };
        }
    }
}
