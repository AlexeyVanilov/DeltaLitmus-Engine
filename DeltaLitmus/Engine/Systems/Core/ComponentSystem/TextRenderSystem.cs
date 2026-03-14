using DeltaLitmus.Components.Core;
using DeltaLitmus.Components.UI;
using DeltaLitmus.Core.SceneSystem;
using DeltaLitmus.Systems.Services.Utils;
using Microsoft.Xna.Framework.Graphics;

namespace DeltaLitmus.Systems.Core.ComponentSystem
{
    public sealed class TextRenderSystem : IEcsDrawSystem {
        public void Draw(EcsWorld world, SpriteBatch spriteBatch) {
            var textStorage = world.GetStorage<TextComponent>();
            var transformStorage = world.GetStorage<TransformComponent>();

            var texts = textStorage.AsSpan();

            for (int i = 0; i < texts.Length; i++)
            {
                ref var txt = ref texts[i];
                if (string.IsNullOrEmpty(txt.Content) || txt.Font == null) continue;

                int entityId = textStorage.GetEntityId(i);
                ref var tr = ref transformStorage.Get(entityId);

                spriteBatch.DrawString(
                    txt.Font,
                    txt.Content,
                    tr.Position,
                    txt.Color,
                    tr.Rotation,
                    txt.Origin,
                    tr.Scale,
                    SpriteEffects.None,
                    0f
                );
            }
        }
    }
}
