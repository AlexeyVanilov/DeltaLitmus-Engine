using DeltaLitmus.Components.Core;
using DeltaLitmus.Core.SceneSystem;
using DeltaLitmus.Engine.Components.Core;
using DeltaLitmus.Systems.Core.MultiThreading;
using DeltaLitmus.Systems.Services.Utils;

namespace DeltaLitmus.Systems.Core.ComponentSystem {
    public sealed class RectUpdateSystem : IEcsSystem {
        public void Update(EcsWorld world, float dt, EntityCommandBuffer ecb) {
            var entities = world.Query<TransformComponent, RectComponent>();
            var trs = world.GetStorage<TransformComponent>();
            var rects = world.GetStorage<RectComponent>();

            for (int i = 0; i < entities.Length; i++)
            {
                int id = entities[i];
                ref var tr = ref trs.Get(id);
                if (!tr.IsDirty) continue;

                ref var r = ref rects.Get(id);
                r.rect.X = (int)tr.Position.X;
                r.rect.Y = (int)tr.Position.Y;
            }
        }
    }
}