using System;
using System.Collections.Generic;

namespace ArtHistoryMap.Api.DTOs
{
    public class GraphDto
    {
        public List<GraphNodeDto> Nodes { get; set; } = new();
        public List<GraphLinkDto> Links { get; set; } = new();
    }

    public class GraphNodeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public string Region { get; set; } = string.Empty;
    }

    // D3 force simulation "source" ve "target" bekler.
    // Kural: Source <RelationType> Target (Source = sonra gelen akım).
    public class GraphLinkDto
    {
        public Guid Source { get; set; }
        public Guid Target { get; set; }
        public string RelationType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
