using DeltaLitmus.Systems.Services.Utils;
using System;
using System.Runtime.CompilerServices;

namespace DeltaLitmus.Systems.Utils
{
    public sealed class ComponentStorage<T> : IStorage where T : struct
    {
        private T[] _components = new T[512];
        private int[] _entityToIdx = new int[1024];
        private int[] _idxToEntity = new int[1024];
        private int _count;

        public ComponentStorage() => Array.Fill(_entityToIdx, -1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has(int entityId) => entityId < _entityToIdx.Length && _entityToIdx[entityId] != -1;

        public ref T Add(int entityId)
        {
            if (entityId >= _entityToIdx.Length) Resize(entityId * 2);
            _entityToIdx[entityId] = _count;
            _idxToEntity[_count] = entityId;
            return ref _components[_count++];
        }

        public void Remove(int entityId)
        {
            if (!Has(entityId)) return;
            int idx = _entityToIdx[entityId];
            int lastIdx = --_count;
            _components[idx] = _components[lastIdx];
            int lastEntity = _idxToEntity[lastIdx];
            _idxToEntity[idx] = lastEntity;
            _entityToIdx[lastEntity] = idx;
            _entityToIdx[entityId] = -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Get(int entityId) => ref _components[_entityToIdx[entityId]];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetEntityId(int index) => _idxToEntity[index];

        public Span<T> AsSpan() => _components.AsSpan(0, _count);

        private void Resize(int size)
        {
            Array.Resize(ref _components, size);
            int old = _entityToIdx.Length;
            Array.Resize(ref _entityToIdx, size);
            Array.Resize(ref _idxToEntity, size);
            for (int i = old; i < size; i++) _entityToIdx[i] = -1;
        }
    }
}