using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DeltaLitmus.Components.UI {
    public struct TextComponent {
        public string Content;
        public SpriteFont Font;
        public Color Color;
        public Vector2 Size;
        public Vector2 Origin;
        public bool IsDirty;

        public void SetText(string newText) {
            if (Content == newText) return;
            Content = newText;
            IsDirty = true;
        }
    }
}