using System;

namespace ArtHistoryMap.Api.DTOs
{
    public class ArtMovementSummaryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public string Region { get; set; } = string.Empty;
    }
}
