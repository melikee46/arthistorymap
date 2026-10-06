using System;
using System.Collections.Generic;

namespace ArtHistoryMap.Api.Entities
{
    public class ArtMovement
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;

        // Navigation property: bu akımın KAYNAK olduğu ilişkiler
        // (yani "bu akım, şu akımları etkiledi / şu akımlara tepki oldu" yönündeki kayıtlar)
        public ICollection<ArtMovementRelation> OutgoingRelations { get; set; } = new List<ArtMovementRelation>();

        // Navigation property: bu akımın HEDEF olduğu ilişkiler
        // (yani "şu akımlar, bu akımı etkiledi" yönündeki kayıtlar)
        public ICollection<ArtMovementRelation> IncomingRelations { get; set; } = new List<ArtMovementRelation>();

        // Bu akıma ait sanatçılar (many-to-many, ArtistMovement join table üzerinden)
        public ICollection<ArtistMovement> ArtistMovements { get; set; } = new List<ArtistMovement>();
    }
}
