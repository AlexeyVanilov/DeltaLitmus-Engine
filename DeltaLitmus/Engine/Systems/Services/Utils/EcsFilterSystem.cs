using DeltaLitmus.Core.SceneSystem;
using DeltaLitmus.Systems.Core.MultiThreading;
using DeltaLitmus.Systems.Services.Utils;
using System;
using System.Collections.Generic;

namespace DeltaLitmus.Systems.Services.Utils {
    public abstract class EcsFilterSystem<T1, T2> : IEcsSystem
    where T1 : struct
    where T2 : struct
    {
        protected readonly List<int> Entities = new(1024);

        public virtual void Update(EcsWorld world, float deltaTime, EntityCommandBuffer ecb)
        {
            var storage1 = world.GetStorage<T1>();
            var storage2 = world.GetStorage<T2>();

            var span1 = storage1.AsSpan();
            var span2 = storage2.AsSpan();

            Execute(span1, span2, deltaTime);
        }

        protected abstract void Execute(Span<T1> c1, Span<T2> c2, float dt);
    }
}
