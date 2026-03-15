using DeltaLitmus.Core.SceneSystem;
using DeltaLitmus.Systems.Core.MultiThreading;
using DeltaLitmus.Systems.Services.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DeltaLitmus.Systems.Core.SceneSystem
{
    public abstract class Scene : IDisposable
    {
        protected readonly EcsWorld World = new();
        protected readonly List<IEcsSystem> FixedSystems = new(128);
        protected readonly List<IEcsSystem> UpdateSystems = new(64);
        protected readonly List<IEcsDrawSystem> DrawSystems = new(128);
        protected readonly List<IEcsDrawSystem> UiDrawSystems = new(64);

        private readonly EntityCommandBuffer _ecb = new();

        public virtual void FixedUpdate(float fixedDt)
        {
            var systems = CollectionsMarshal.AsSpan(FixedSystems);
            for (int i = 0; i < systems.Length; i++)
                systems[i].Update(World, fixedDt, _ecb);

            _ecb.Playback(World);
        }

        public virtual void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var systems = CollectionsMarshal.AsSpan(UpdateSystems);
            for (int i = 0; i < systems.Length; i++)
                systems[i].Update(World, dt, _ecb);

            _ecb.Playback(World);
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch sb, float alpha)
        {
            var cam = World.Camera;
            if (cam == null) return;

            sb.Begin(transformMatrix: cam.GetViewMatrix(), samplerState: SamplerState.PointClamp);
            Render(DrawSystems, sb);
            sb.End();

            sb.Begin(samplerState: SamplerState.PointClamp);
            Render(UiDrawSystems, sb);
            sb.End();
        }
        private void Render(List<IEcsDrawSystem> list, SpriteBatch sb)
        {
            var systems = CollectionsMarshal.AsSpan(list);
            for (int i = 0; i < systems.Length; i++)
                systems[i].Draw(World, sb);
        }

        public abstract void Load();

        public virtual void Dispose()
        {
            FixedSystems.Clear();
            UpdateSystems.Clear();
            DrawSystems.Clear();
            UiDrawSystems.Clear();
            World.Dispose();
        }
    }
}