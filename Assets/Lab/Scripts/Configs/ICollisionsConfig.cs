using Multi2D.Data.Collisions;
using System.Collections.Generic;

namespace Multi2D
{
    public interface ICollisionsConfig
    {
        public IReadOnlyList<CollisionLayerMask> CollisionLayers { get; }
    }
}
