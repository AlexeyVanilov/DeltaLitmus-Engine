using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace DeltaLitmus.Services
{
    public static class AssetManager
    {
        private static ContentManager _content;
        private const string spritesCategory = "Sprites";
        private const string sfxCategory = "SFX";
        private const string fontsCategory = "Fonts";

        private static readonly Dictionary<string, object> _cache = new(128);
        public static void Init(ContentManager content) => _content = content;

        public static T Get<T>(string path)
        {
            if (_cache.TryGetValue(path, out var asset))
                return (T)asset;

            T newAsset = _content.Load<T>(path);
            _cache.Add(path, newAsset);
            return newAsset;
        }
        
        public static void Unload() {
            _cache.Clear();
            _content.Unload();
        }
        public static Texture2D GetTexture(string name) => Get<Texture2D>($"{spritesCategory}/{name}");
        public static SpriteFont GetFont(string name) => Get<SpriteFont>($"{fontsCategory}/{name}");
        public static SoundEffect GetSound(string name) => Get<SoundEffect>($"{sfxCategory}/{name}");
    }
}