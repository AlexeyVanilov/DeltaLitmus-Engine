using DeltaLitmus.Components.Core;
using DeltaLitmus.Components.UI;
using DeltaLitmus.Core.SceneSystem;
using DeltaLitmus.Engine.Components.Core;
using DeltaLitmus.Systems.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.CompilerServices;

namespace DeltaLitmus.Systems.Services.Utils
{
    public static class EntityFactory
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ref TransformComponent InitTransform(EcsWorld world, Entity entity, Vector2 pos, int parentId = -1)
        {
            ref var tr = ref world.Add<TransformComponent>(entity);
            tr.Position = pos;
            tr.Scale = Vector2.One;
            tr.ParentId = parentId;
            tr.IsDirty = true;
            return ref tr;
        }

        public static Entity CreateSprite(EcsWorld world, Texture2D tex, Vector2 pos, Color color)
        {
            var entity = world.CreateEntity();
            InitTransform(world, entity, pos);

            ref var img = ref world.Add<ImageComponent>(entity);
            img.Texture = tex;
            img.Color = color;
            img.Origin = new Vector2(tex.Width * 0.5f, tex.Height * 0.5f);

            return entity;
        }

        public static Entity CreateButton(EcsWorld world, Texture2D tex, Color col, Vector2 pos, SpriteFont font, string txt, Color txtCol)
        {
            var btn = world.CreateEntity();
            InitTransform(world, btn, pos);

            ref var img = ref world.Add<ImageComponent>(btn);
            img.Texture = tex;
            img.Color = col;
            img.Origin = new Vector2(tex.Width * 0.5f, tex.Height * 0.5f);

            ref var rect = ref world.Add<RectComponent>(btn);
            rect.rect = new Rectangle((int)(pos.X - img.Origin.X), (int)(pos.Y - img.Origin.Y), tex.Width, tex.Height);

            var text = CreateText(world, font, txt, Vector2.Zero, txtCol);
            world.GetStorage<TransformComponent>().Get(text.Id).ParentId = btn.Id;

            return btn;
        }

        public static Entity CreateText(EcsWorld world, SpriteFont font, string content, Vector2 pos, Color color)
        {
            var entity = world.CreateEntity();
            InitTransform(world, entity, pos);

            ref var txt = ref world.Add<TextComponent>(entity);
            txt.Font = font;
            txt.Content = content;
            txt.Color = color;

            return entity;
        }
    }
}