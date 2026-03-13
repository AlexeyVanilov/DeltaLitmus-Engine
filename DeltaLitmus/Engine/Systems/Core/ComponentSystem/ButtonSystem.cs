using DeltaLitmus.Components.UI;
using DeltaLitmus.Core.SceneSystem;
using DeltaLitmus.Engine.Components.Core;
using DeltaLitmus.Engine.Components.UI;
using DeltaLitmus.Services;
using DeltaLitmus.Systems.Core.MultiThreading;
using DeltaLitmus.Systems.Services.Utils;
using Microsoft.Xna.Framework;

namespace DeltaLitmus.Systems.Core.ComponentSystem
{
    public sealed class ButtonSystem : IEcsSystem
    {
        public void Update(EcsWorld world, float dt, EntityCommandBuffer ecb)
        {
            var buttons = world.Query<ButtonComponent, RectComponent, ImageComponent>();

            var bStorage = world.GetStorage<ButtonComponent>();
            var rStorage = world.GetStorage<RectComponent>();
            var iStorage = world.GetStorage<ImageComponent>();

            Vector2 mousePos = InputManager.MousePosition;
            bool isClick = InputManager.IsMouseButtonPressed(MouseButton.Left);

            for (int i = 0; i < buttons.Length; i++)
            {
                int id = buttons[i];
                ref var btn = ref bStorage.Get(id);
                ref var rect = ref rStorage.Get(id);
                ref var img = ref iStorage.Get(id);

                if (rect.rect.Contains(mousePos.ToPoint()))
                {
                    img.Color = btn.HoveredColor;
                    if (isClick) btn.OnClicked?.Invoke();
                }
                else
                {
                    img.Color = btn.DefaultColor;
                }
            }
        }
    }
}