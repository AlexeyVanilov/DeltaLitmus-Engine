using DeltaLitmus.Components.Core;
using DeltaLitmus.Systems.Core;
using DeltaLitmus.Systems.Services.Utils;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DeltaLitmus.Core.SceneSystem
{
    public sealed class EcsWorld {
        public Camera2D Camera;

        private int _nextId;
        private readonly Stack<int> _freeIds = new(1024);

        private long[] _masks = new long[1024];
        private int[] _versions = new int[1024];

        private readonly Dictionary<Type, object> _storages = new();
        private readonly List<int> _queryCache = new(1024);

        public Entity CreateEntity()
        {
            int id;
            if (_freeIds.Count > 0)
            {
                id = _freeIds.Pop();
            }
            else
            {
                id = _nextId++;
                if (id >= _masks.Length)
                {
                    Array.Resize(ref _masks, _masks.Length * 2);
                    Array.Resize(ref _versions, _versions.Length * 2);
                }
            }

            _masks[id] = 0;
            return new Entity(id, _versions[id]);
        }

        public void DestroyEntity(Entity entity)
        {
            int id = entity.Id;
            if (id < 0 || id >= _nextId || _versions[id] != entity.Version) return;

            foreach (var storage in _storages.Values)
            {
                ((IStorage)storage).Remove(id);
            }

            _masks[id] = 0;
            _versions[id]++;
            _freeIds.Push(id);
        }

        public bool IsAlive(Entity entity)
        {
            return entity.Id >= 0 && entity.Id < _nextId && _versions[entity.Id] == entity.Version && _masks[entity.Id] != 0;
        }
        public ComponentStorage<T> GetStorage<T>() where T : struct
        {
            if (!_storages.TryGetValue(typeof(T), out var storage))
            {
                storage = new ComponentStorage<T>();
                _storages[typeof(T)] = storage;
            }
            return (ComponentStorage<T>)storage;
        }

        public Entity GetEntity(int id) => new Entity(id, _versions[id]);

        public ref T Add<T>(Entity entity) where T : struct
        {
            _masks[entity.Id] |= (1L << ComponentMeta<T>.Id);
            return ref GetStorage<T>().Add(entity.Id);
        }

        public void Remove<T>(Entity entity) where T : struct
        {
            _masks[entity.Id] &= ~(1L << ComponentMeta<T>.Id);
            GetStorage<T>().Remove(entity.Id);
        }

        public ReadOnlySpan<int> Query<T1>() where T1 : struct
        {
            _queryCache.Clear();
            long targetMask = 1L << ComponentMeta<T1>.Id;
            for (int i = 0; i < _nextId; i++)
                if ((_masks[i] & targetMask) == targetMask) _queryCache.Add(i);
            return CollectionsMarshal.AsSpan(_queryCache);
        }

        public ReadOnlySpan<int> Query<T1, T2>() where T1 : struct where T2 : struct
        {
            _queryCache.Clear();
            long targetMask = (1L << ComponentMeta<T1>.Id) | (1L << ComponentMeta<T2>.Id);
            for (int i = 0; i < _nextId; i++)
                if ((_masks[i] & targetMask) == targetMask) _queryCache.Add(i);
            return CollectionsMarshal.AsSpan(_queryCache);
        }

        public ReadOnlySpan<int> Query<T1, T2, T3>() where T1 : struct where T2 : struct where T3 : struct
        {
            _queryCache.Clear();
            long targetMask = (1L << ComponentMeta<T1>.Id) | (1L << ComponentMeta<T2>.Id) | (1L << ComponentMeta<T3>.Id);
            for (int i = 0; i < _nextId; i++)
                if ((_masks[i] & targetMask) == targetMask) _queryCache.Add(i);
            return CollectionsMarshal.AsSpan(_queryCache);
        }
    }
}
