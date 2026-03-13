using DeltaLitmus.Components.Core;
using DeltaLitmus.Core.SceneSystem;
using DeltaLitmus.Systems.Core.MultiThreading;
using DeltaLitmus.Systems.Services.Utils;
using DeltaLitmus.Systems.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DeltaLitmus.Systems.Core.SceneSystem {
    public abstract class Scene : IDisposable {
        protected readonly EcsWorld World = new();
        protected readonly List<IEcsSystem> UpdateSystems = new(128);
        protected readonly List<IEcsDrawSystem> DrawSystems = new(128);
        protected readonly List<IEcsDrawSystem> UiDrawSystems = new(64);

        public Camera2D Camera {
            get => World.Camera;
        }

        public virtual void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var systems = CollectionsMarshal.AsSpan(UpdateSystems);

            var ecb = new EntityCommandBuffer();

            for (int i = 0; i < systems.Length; i++)
            {
                systems[i].Update(World, dt, ecb);
            }

            ecb.Playback(World);
        }

        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            if (Camera == null) return;

            sb.Begin(transformMatrix: Camera.GetViewMatrix(), samplerState: SamplerState.PointClamp);
            Render(DrawSystems, sb);
            sb.End();

            sb.Begin(samplerState: SamplerState.PointClamp);
            Render(UiDrawSystems, sb);
            sb.End();
        }

        private void Render(List<IEcsDrawSystem> list, SpriteBatch sb)
        {
            var systems = CollectionsMarshal.AsSpan(list);
            for (int i = 0; i < systems.Length; i++) systems[i].Draw(World, sb);
        }

        public void SetCamera(Camera2D cam) => World.Camera = cam;
        public abstract void Load();
        public virtual void Dispose() { UpdateSystems.Clear(); DrawSystems.Clear(); UiDrawSystems.Clear(); }
    }
}