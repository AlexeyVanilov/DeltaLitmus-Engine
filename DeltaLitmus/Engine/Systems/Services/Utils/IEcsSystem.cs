using DeltaLitmus.Core.SceneSystem;
using DeltaLitmus.Systems.Core.MultiThreading;

namespace DeltaLitmus.Systems.Services.Utils {
    public interface IEcsSystem {
        void Update(EcsWorld world, float dt, EntityCommandBuffer ecb);
    }
}