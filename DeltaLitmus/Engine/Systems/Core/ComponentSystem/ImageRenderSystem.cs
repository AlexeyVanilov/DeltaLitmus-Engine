using DeltaLitmus.Components.Core;
using DeltaLitmus.Components.UI;
using DeltaLitmus.Core.SceneSystem;
using Microsoft.Xna.Framework.Graphics;

namespace DeltaLitmus.Systems.Core.ComponentSystem
{
    public sealed class ImageRenderSystem {
        public void Draw(EcsWorld world, SpriteBatch spriteBatch)
        {
            var imageStorage = world.GetStorage<ImageComponent>();
            var transformStorage = world.GetStorage<TransformComponent>();

            var images = imageStorage.AsSpan();

            for (int i = 0; i < images.Length; i++)
            {
                ref var img = ref images[i];
                if (img.Texture == null) continue;

                int entityId = imageStorage.GetEntityId(i);
                ref var tr = ref transformStorage.Get(entityId);

                spriteBatch.Draw(
                    img.Texture,
                    tr.Position,
                    img.SourceRectangle,
                    img.Color,
                    tr.Rotation,
                    img.Origin,
                    tr.Scale,
                    SpriteEffects.None,
                    0f
                );
            }
        }
    }
}
