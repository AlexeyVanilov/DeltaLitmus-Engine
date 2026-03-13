using DeltaLitmus.Components.Core;
using DeltaLitmus.Core.SceneSystem;
using DeltaLitmus.Systems.Core.MultiThreading;
using DeltaLitmus.Systems.Services.Utils;

namespace DeltaLitmus.Systems.Core.ComponentSystem
{
    public sealed class TransformSystem : IEcsSystem
    {
        public void Update(EcsWorld world, float deltaTime, EntityCommandBuffer ecb)
        {
            var storage = world.GetStorage<TransformComponent>();
            var transforms = storage.AsSpan();

            for (int i = 0; i < transforms.Length; i++)
            {
                ref var tr = ref transforms[i];

                if (!tr.IsDirty && tr.ParentId == -1) continue;

                if (tr.ParentId != -1)
                {
                    ref var parent = ref storage.Get(tr.ParentId);
                    if (parent.IsDirty) tr.IsDirty = true;
                }

                if (tr.IsDirty)
                {
                    UpdateMatrix(ref tr);

                    if (tr.ParentId != -1)
                    {
                        tr.WorldMatrix *= storage.Get(tr.ParentId).WorldMatrix;
                    }

                    tr.IsDirty = false;
                }
            }
        }

        private static void UpdateMatrix(ref TransformComponent tr)
        {
            Microsoft.Xna.Framework.Matrix.CreateScale(tr.Scale.X, tr.Scale.Y, 1f, out var s);
            Microsoft.Xna.Framework.Matrix.CreateRotationZ(tr.Rotation, out var r);
            Microsoft.Xna.Framework.Matrix.CreateTranslation(tr.Position.X, tr.Position.Y, 0f, out var t);

            Microsoft.Xna.Framework.Matrix.Multiply(ref s, ref r, out var sr);
            Microsoft.Xna.Framework.Matrix.Multiply(ref sr, ref t, out tr.WorldMatrix);
        }
    }
}
