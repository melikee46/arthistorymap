using System;

namespace ArtHistoryMap.Api.Entities
{
    public class Artwork
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int? Year { get; set; }
        public string ImageUrl { get; set; } = string.Empty;

        public Guid ArtistId { get; set; }
        public Artist Artist { get; set; } = null!;
    }
}
