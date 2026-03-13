using DeltaLitmus.Components.Core;
using DeltaLitmus.Components.UI;
using DeltaLitmus.Core.SceneSystem;
using DeltaLitmus.Engine.Components.Core;
using DeltaLitmus.Systems.Core.MultiThreading;
using DeltaLitmus.Systems.Services.Utils;
using Microsoft.Xna.Framework;
using System;

namespace DeltaLitmus.Systems.Core.ComponentSystem
{
    public sealed class TextSystem : IEcsSystem
    {
        public void Update(EcsWorld world, float deltaTime, EntityCommandBuffer ecb)
        {
            var textStorage = world.GetStorage<TextComponent>();
            var transformStorage = world.GetStorage<TransformComponent>();
            var rectStorage = world.GetStorage<RectComponent>();

            var entities = world.Query<TextComponent, TransformComponent, RectComponent>();

            foreach (int entityId in entities)
            {
                ref var txt = ref textStorage.Get(entityId);
                ref var tr = ref transformStorage.Get(entityId);
                ref var rct = ref rectStorage.Get(entityId);

                if (txt.IsDirty)
                {
                    UpdateMetrics(ref txt, ref tr);
                }

                if (txt.IsDirty || tr.IsDirty)
                {
                    UpdateBounds(ref rct, ref txt, ref tr);
                    txt.IsDirty = false;
                }
            }
        }

        private static void UpdateMetrics(ref TextComponent txt, ref TransformComponent tr)
        {
            if (txt.Font == null || string.IsNullOrEmpty(txt.Content))
            {
                txt.Size = Vector2.Zero;
                txt.Origin = Vector2.Zero;
                return;
            }

            txt.Size = txt.Font.MeasureString(txt.Content);
            txt.Origin = new Vector2(
                (float)Math.Floor(txt.Size.X * 0.5f),
                (float)Math.Floor(txt.Size.Y * 0.5f)
            );
        }

        private static void UpdateBounds(ref RectComponent rect, ref TextComponent txt, ref TransformComponent tr)
        {
            rect.rect = new Rectangle(
                (int)(tr.Position.X - txt.Origin.X * tr.Scale.X),
                (int)(tr.Position.Y - txt.Origin.Y * tr.Scale.Y),
                (int)(txt.Size.X * tr.Scale.X),
                (int)(txt.Size.Y * tr.Scale.Y)
            );
        }
    }
}