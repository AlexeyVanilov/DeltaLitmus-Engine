using DeltaLitmus.Components.Core;
using DeltaLitmus.Components.Core.DragDrop;
using DeltaLitmus.Core.SceneSystem;
using DeltaLitmus.Engine.Components.Core;
using DeltaLitmus.Services;
using DeltaLitmus.Systems.Core.MultiThreading;
using DeltaLitmus.Systems.Services.Utils;
using Microsoft.Xna.Framework;
using System;

namespace DeltaLitmus.Systems.Core.ComponentSystem
{
    public sealed class InteractionSystem : IEcsSystem
    {
        private Entity _activeEntity = default;
        private bool _isDragging = false;

        public void Update(EcsWorld world, float dt, EntityCommandBuffer ecb)
        {
            var draggables = world.Query<DraggableComponent, RectComponent, TransformComponent>();

            var dragStorage = world.GetStorage<DraggableComponent>();
            var rectStorage = world.GetStorage<RectComponent>();
            var trStorage = world.GetStorage<TransformComponent>();

            if (!_isDragging)
            {
                MouseButton? pressedBtn = null;
                if (InputManager.IsMouseButtonPressed(MouseButton.Left)) pressedBtn = MouseButton.Left;
                else if (InputManager.IsMouseButtonPressed(MouseButton.Right)) pressedBtn = MouseButton.Right;

                if (pressedBtn.HasValue)
                {
                    TryBegin(world, draggables, pressedBtn.Value, ecb);
                }
            }
            else
            {
                if (!world.IsAlive(_activeEntity))
                {
                    _isDragging = false;
                    return;
                }

                int id = _activeEntity.Id;
                ref var drag = ref dragStorage.Get(id);
                ref var tr = ref trStorage.Get(id);

                var stateStorage = world.GetStorage<DragStateComponent>();
                if (!stateStorage.Has(id)) return;

                ref var state = ref stateStorage.Get(id);

                if (InputManager.IsMouseButtonDown(state.Button))
                {
                    Vector2 mPos = drag.IsUi ? InputManager.MousePosition : world.Camera.ScreenToWorld(InputManager.MousePosition);
                    tr.Position = mPos + state.Offset;
                    tr.IsDirty = true;
                }
                else
                {
                    ecb.Remove<DragStateComponent>(_activeEntity);
                    _isDragging = false;
                    _activeEntity = default;
                }
            }
        }

        private void TryBegin(EcsWorld world, ReadOnlySpan<int> entities, MouseButton btn, EntityCommandBuffer ecb)
        {
            for (int i = entities.Length - 1; i >= 0; i--)
            {
                int id = entities[i];
                var drag = world.GetStorage<DraggableComponent>().Get(id);
                Vector2 mPos = drag.IsUi ? InputManager.MousePosition : world.Camera.ScreenToWorld(InputManager.MousePosition);

                if (world.GetStorage<RectComponent>().Get(id).rect.Contains(mPos.ToPoint()))
                {
                    var tr = world.GetStorage<TransformComponent>().Get(id);

                    _activeEntity = world.GetEntity(id);
                    _isDragging = true;

                    ecb.Add(_activeEntity, new DragStateComponent
                    {
                        Button = btn,
                        Offset = tr.Position - mPos
                    });
                    break;
                }
            }
        }
    }
}