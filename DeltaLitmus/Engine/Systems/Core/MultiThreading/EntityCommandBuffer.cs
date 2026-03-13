using DeltaLitmus.Core.SceneSystem;
using System.Collections.Concurrent;

namespace DeltaLitmus.Systems.Core.MultiThreading
{
    public sealed class EntityCommandBuffer
    {
        private readonly ConcurrentQueue<ICommand> _commands = new();

        public void Add<T>(Entity entity, T component) where T : struct
        {
            _commands.Enqueue(new AddComponentCommand<T> { Entity = entity, Component = component });
        }
        public void Remove<T>(Entity entity) where T : struct
        {
            _commands.Enqueue(new RemoveComponentCommand<T> { Entity = entity });
        }

        public void Playback(EcsWorld world)
        {
            while (_commands.TryDequeue(out var cmd))
            {
                cmd.Execute(world);
            }
        }
    }
}