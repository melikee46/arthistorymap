using System;

namespace ArtHistoryMap.Api.Entities
{
    public class ArtistMovement
    {
        public Guid ArtistId { get; set; }
        public Artist Artist { get; set; } = null!;

        public Guid MovementId { get; set; }
        public ArtMovement Movement { get; set; } = null!;
    }
}
