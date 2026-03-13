using DeltaLitmus.Core.SceneSystem;
using DeltaLitmus.Services.Managers;
using DeltaLitmus.Systems.Core.MultiThreading;
using DeltaLitmus.Systems.Services.Utils;
using DeltaLitmus.Systems.Utils;
using System.Collections.Generic;

namespace DeltaLitmus.Systems.Core.ComponentSystem
{
    public sealed class SoundSystem : IEcsSystem
    {
        private readonly AudioManager _audio;
        private readonly HashSet<int> _playedThisFrame = new();

        public SoundSystem(AudioManager audio) => _audio = audio;

        public void Update(EcsWorld world, float dt, EntityCommandBuffer ecb)
        {
            var entities = world.Query<SoundRequestComponent>();
            var storage = world.GetStorage<SoundRequestComponent>();

            _playedThisFrame.Clear();

            for (int i = 0; i < entities.Length; i++)
            {
                int id = entities[i];
                ref var req = ref storage.Get(id);
                int clipHash = req.ClipName.GetHashCode();

                if (!_playedThisFrame.Contains(clipHash))
                {
                    _audio.Play(req.ClipName, req.Volume, req.Pitch);
                    _playedThisFrame.Add(clipHash);
                }

                ecb.Remove<SoundRequestComponent>(world.GetEntity(id));
            }
        }
    }
}