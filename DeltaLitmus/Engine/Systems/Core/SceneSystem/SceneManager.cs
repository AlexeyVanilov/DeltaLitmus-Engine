using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace DeltaLitmus.Systems.Core.SceneSystem
{
    public static class SceneManager
    {
        private static readonly List<Scene> _scenes = new(16);
        private static Scene _activeScene;
        private static int _nextSceneIndex = -1;

        public static void RegisterScene(Scene scene) => _scenes.Add(scene);

        public static void LoadScene(byte index) => _nextSceneIndex = index;

        public static void FixedUpdate(float fixedTime) => _activeScene?.FixedUpdate(fixedTime);
        public static void Update(GameTime gt) => _activeScene?.Update(gt);
        public static void Draw(GameTime gt, SpriteBatch sb, float alpha) => _activeScene?.Draw(gt, sb, alpha);
    }
}