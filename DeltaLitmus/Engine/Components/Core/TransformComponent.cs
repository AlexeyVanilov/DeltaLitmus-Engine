using Microsoft.Xna.Framework;

namespace DeltaLitmus.Components.Core {
    public struct TransformComponent {
        public Vector2 Position;
        public Vector2 Scale;
        public float Rotation;
        public Matrix WorldMatrix;
        public int ParentId;
        public bool IsDirty;
    }
}