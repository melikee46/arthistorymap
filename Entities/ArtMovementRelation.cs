using System;

namespace ArtHistoryMap.Api.Entities
{
    public enum RelationType
    {
        InfluencedBy = 0,
        ReactionTo = 1,
        ContemporaryWith = 2
    }

    public class ArtMovementRelation
    {
        public Guid Id { get; set; }

        public Guid SourceMovementId { get; set; }
        public ArtMovement SourceMovement { get; set; } = null!;

        public Guid TargetMovementId { get; set; }
        public ArtMovement TargetMovement { get; set; } = null!;

        public RelationType RelationType { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
