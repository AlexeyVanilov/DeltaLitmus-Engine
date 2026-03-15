using System;
using System.Runtime.CompilerServices;

namespace DeltaLitmus.Systems.Services.Utils
{
    public sealed class ComponentStorage<T> : IStorage where T : struct
    {
        private T[] _components = new T[512];
        private int[] _entityToIdx = new int[1024];
        private int[] _idxToEntity = new int[1024];
        private int _count;

        public int Count => _count;

        public ComponentStorage() => Array.Fill(_entityToIdx, -1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has(int entityId) => entityId < _entityToIdx.Length && _entityToIdx[entityId] != -1;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetEntityId(int index) => _idxToEntity[index];
        public ref T Add(int entityId)
        {
            if (entityId >= _entityToIdx.Length) Resize(entityId + 1); 

            if (_entityToIdx[entityId] != -1) return ref _components[_entityToIdx[entityId]];

            if (_count >= _components.Length) ResizeDense(_components.Length * 2);

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
            _components[lastIdx] = default;
            int lastEntity = _idxToEntity[lastIdx];
            _idxToEntity[idx] = lastEntity;
            _entityToIdx[lastEntity] = idx;

            _entityToIdx[entityId] = -1;
        }

        public void Clear() {
            Array.Clear(_components, 0, _count);
            for (int i = 0; i < _count; i++)
            {
                _entityToIdx[_idxToEntity[i]] = -1;
            }
            _count = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Get(int entityId) => ref _components[_entityToIdx[entityId]];

        public Span<T> AsSpan() => _components.AsSpan(0, _count);

        private void Resize(int size)
        {
            int old = _entityToIdx.Length;
            int newSize = Math.Max(size, old * 2);

            Array.Resize(ref _entityToIdx, newSize);
            Array.Resize(ref _idxToEntity, newSize);

            for (int i = old; i < newSize; i++) _entityToIdx[i] = -1;

            if (newSize > _components.Length) ResizeDense(newSize);
        }

        private void ResizeDense(int size)
        {
            Array.Resize(ref _components, size);
        }
    }
}