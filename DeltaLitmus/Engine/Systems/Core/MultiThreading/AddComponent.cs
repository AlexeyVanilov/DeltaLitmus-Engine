using DeltaLitmus.Core.SceneSystem;

namespace DeltaLitmus.Systems.Core.MultiThreading {
    public struct AddComponentCommand<T> : ICommand where T : struct {
        public Entity Entity;
        public T Component;
        public void Execute(EcsWorld world) => world.Add<T>(Entity) = Component;
    }
}
