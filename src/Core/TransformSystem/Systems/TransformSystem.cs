using Arch.Core;

namespace DeltaLitmus.Core.TransformSystem {
    public sealed class TransformSystem {
        private readonly QueryDescription _movementQuery = new QueryDescription()
            .WithAll<TransformComponent, VelocityComponent>();

        public void Update(World world, float deltaTime) {
            world.Query(in _movementQuery, (ref TransformComponent transform, ref VelocityComponent velocity) => {
                transform.Position += velocity.Velocity * deltaTime;
            });
        }
    }
}