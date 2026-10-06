using System;
using System.Collections.Generic;

namespace ArtHistoryMap.Api.Entities
{
    public class Artist
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? BirthYear { get; set; }
        public int? DeathYear { get; set; }
        public string Nationality { get; set; } = string.Empty;

        public ICollection<ArtistMovement> ArtistMovements { get; set; } = new List<ArtistMovement>();
        public ICollection<Artwork> Artworks { get; set; } = new List<Artwork>();
    }
}
