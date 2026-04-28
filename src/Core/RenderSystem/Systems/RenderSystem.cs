using Arch.Core;
using DeltaLitmus.Core.TransformSystem;
using Microsoft.Xna.Framework.Graphics;

namespace DeltaLitmus.Core.RenderSystem {
    public sealed class RenderSystem {
        private readonly QueryDescription _renderQuery = new QueryDescription()
            .WithAll<TransformComponent, SpriteComponent>();

        public void Draw(World world, SpriteBatch sb) {
            world.Query(in _renderQuery, (ref TransformComponent transform, ref SpriteComponent sprite) => {
                sb.Begin();
                sb.Draw(sprite.Texture, transform.Position, sprite.Color);
                sb.End();
            });
        }
    }
}