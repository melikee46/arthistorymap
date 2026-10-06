using System;
using System.Collections.Generic;

namespace ArtHistoryMap.Api.DTOs
{
    public class ArtMovementDetailDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;

        public List<RelationDto> OutgoingRelations { get; set; } = new();
        public List<RelationDto> IncomingRelations { get; set; } = new();
        public List<string> ArtistNames { get; set; } = new();
    }

    public class RelationDto
    {
        public Guid RelatedMovementId { get; set; }
        public string RelatedMovementName { get; set; } = string.Empty;
        public string RelationType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
