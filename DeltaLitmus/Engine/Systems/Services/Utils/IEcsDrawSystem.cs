using DeltaLitmus.Core.SceneSystem;
using Microsoft.Xna.Framework.Graphics;

namespace DeltaLitmus.Systems.Services.Utils {
    public interface IEcsDrawSystem {
        void Draw(EcsWorld world, SpriteBatch spriteBatch);
    }
}
