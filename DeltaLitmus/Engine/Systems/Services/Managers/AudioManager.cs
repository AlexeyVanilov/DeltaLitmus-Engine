using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace DeltaLitmus.Services.Managers {
    public sealed class AudioManager {
        private readonly Dictionary<int, SoundEffect> _sfxCache = new();
        private readonly ContentManager _content;

        public AudioManager(ContentManager content) => _content = content;

        public void Play(string asset, float volume, float pitch)
        {
            int hash = asset.GetHashCode();
            if (!_sfxCache.TryGetValue(hash, out var sfx))
            {
                sfx = _content.Load<SoundEffect>(asset);
                _sfxCache[hash] = sfx;
            }
            sfx.Play(volume, pitch, 0);
        }
    }
}