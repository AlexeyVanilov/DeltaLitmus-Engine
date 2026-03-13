using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace DeltaLitmus.Systems.Core.SceneSystem
{
    public static class SceneManager
    {
        private static readonly List<Scene> _scenes = new();
        private static Scene _activeScene;

        public static Scene ActiveScene => _activeScene;

        public static void RegisterScene(Scene scene) => _scenes.Add(scene);

        public static void LoadScene(byte index)
        {
            if (index >= _scenes.Count) return;

            _activeScene?.Dispose();
            _activeScene = _scenes[index];
            _activeScene.Load();
        }

        public static void Update(GameTime gt) => _activeScene?.Update(gt);
        public static void Draw(GameTime gt, SpriteBatch sb) => _activeScene?.Draw(gt, sb);
    }
}