using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeltaLitmus.Components.UI {
    public struct ImageComponent {
        public Texture2D Texture;
        public Color Color;
        public Rectangle? SourceRectangle;
        public Vector2 Origin;
    }
}