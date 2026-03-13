using System;

namespace DeltaLitmus.Systems.Core {
    public readonly struct Entity : IEquatable<Entity> {
        public readonly long PackedId;

        public int Id => (int)(PackedId & 0xFFFFFFFF);
        public int Version => (int)(PackedId >> 32);

        public Entity(int id, int version)
        {
            PackedId = (long)version << 32 | (uint)id;
        }

        public bool Equals(Entity other) => PackedId == other.PackedId;
        public override bool Equals(object obj) => obj is Entity other && Equals(other);
        public override int GetHashCode() => PackedId.GetHashCode();
        public static bool operator ==(Entity left, Entity right) => left.Equals(right);
        public static bool operator !=(Entity left, Entity right) => !left.Equals(right);
    }
}