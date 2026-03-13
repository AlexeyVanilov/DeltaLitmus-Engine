using DeltaLitmus.Components.Core;
using DeltaLitmus.Core.SceneSystem;
using DeltaLitmus.Engine.Components.Core;
using DeltaLitmus.Systems.Core.MultiThreading;
using DeltaLitmus.Systems.Services.Utils;
using Microsoft.Xna.Framework;

namespace DeltaLitmus.Systems.Core.ComponentSystem
{
    public sealed class CollisionSystem : IEcsSystem
    {
        private readonly SpatialGrid _grid = new(128);

        public void Update(EcsWorld world, float dt, EntityCommandBuffer ecb)
        {
            var entities = world.Query<TransformComponent, RectComponent, ColliderComponent>();
            var rects = world.GetStorage<RectComponent>();
            var colliders = world.GetStorage<ColliderComponent>();

            _grid.Clear();

            for (int i = 0; i < entities.Length; i++)
            {
                int id = entities[i];
                _grid.Insert(id, rects.Get(id).rect);
            }

            for (int i = 0; i < entities.Length; i++)
            {
                int id = entities[i];
                ref var cl = ref colliders.Get(id);

                if (cl.isStatic) continue;

                Rectangle r1 = rects.Get(id).rect;
                var nearby = _grid.GetNearby(r1);

                foreach (int otherId in nearby)
                {
                    if (id == otherId) continue;

                    Rectangle r2 = rects.Get(otherId).rect;

                    if (r1.Intersects(r2))
                    {
                        ResolveCollision(world, id, otherId, ecb);
                    }
                }
            }
        }

        private void ResolveCollision(EcsWorld world, int id1, int id2, EntityCommandBuffer ecb)
        {
            Entity e1 = world.GetEntity(id1);
            Entity e2 = world.GetEntity(id2);

            ecb.Add(e1, new CollisionEventComponent { Other = e2 });
            ecb.Add(e2, new CollisionEventComponent { Other = e1 });
        }
    }
}