using DeltaLitmus.Core.SceneSystem;

namespace DeltaLitmus.Systems.Core.MultiThreading {
    public interface ICommand {
        void Execute(EcsWorld world);
    }
}
