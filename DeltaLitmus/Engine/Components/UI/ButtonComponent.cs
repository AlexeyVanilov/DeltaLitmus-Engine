using Microsoft.Xna.Framework;
using System;

namespace DeltaLitmus.Engine.Components.UI
{
    public struct ButtonComponent
    {
        public Color HoveredColor;
        public Color DefaultColor;
        public Action OnClicked;
        public bool IsHovered;
    }
}