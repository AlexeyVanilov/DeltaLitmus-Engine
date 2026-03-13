using DeltaLitmus.Core.SceneSystem;

namespace DeltaLitmus.Systems.Core.MultiThreading {
    public struct RemoveComponentCommand<T> : ICommand where T : struct
    {
        public Entity Entity;
        public void Execute(EcsWorld world) => world.Remove<T>(Entity);
    }
}
