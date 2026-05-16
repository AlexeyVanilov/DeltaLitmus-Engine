using DeltaLitmus.Core.InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace GameEngine.Core.InputSystem {
    public static class InputSystem {
        public const int startMaxKeys = 4;
        private static List<Key> _keys = new List<Key>(startMaxKeys);

        private static MouseState _currentMouseEntry;
        private static MouseState _prevMouseEntry;

        public static int ScrollDelta => _currentMouseEntry.ScrollWheelValue - _prevMouseEntry.ScrollWheelValue;
        public static Vector2 MousePosition => new Vector2(_currentMouseEntry.X, _currentMouseEntry.Y);
        public static Vector2 MouseDelta => new Vector2(_currentMouseEntry.X - _prevMouseEntry.X, _currentMouseEntry.Y - _prevMouseEntry.Y);

        private static KeyboardState _currentKeyState;
        private static KeyboardState _prevKeyState;

        public static void Update()
        {
            _prevMouseEntry = _currentMouseEntry;
            _currentMouseEntry = Mouse.GetState();

            _prevKeyState = _currentKeyState;
            _currentKeyState = Keyboard.GetState();

            for (int i = 0; i < _keys.Count; i++)
            {
                var key = _keys[i].KeyValue;
                if (key == Keys.None) continue;

                bool isDown = _currentKeyState.IsKeyDown(key);
                bool wasDown = _prevKeyState.IsKeyDown(key);

                bool shouldInvoke = _keys[i].KeyType switch
                {
                    KeyType.Pressed => isDown && !wasDown,
                    KeyType.Released => !isDown && wasDown,
                    KeyType.Held => isDown,
                    _ => false
                };

                if (shouldInvoke) _keys[i].KeyAction?.Invoke();
            }
        }

        public static bool IsKeyDown(Keys key) => _currentKeyState.IsKeyDown(key);

        public static void Add(Keys key, KeyType keyType, Action keyAction) {
            Key keyStruct = new Key(key, keyType, keyAction);
            _keys.Add(keyStruct);
        }
        public static void Add(Key keyStruct) => _keys.Add(keyStruct);
        public static bool IsMouseButtonDown(MouseButton button) => button switch
        {
            MouseButton.Left => _currentMouseEntry.LeftButton == ButtonState.Pressed,
            MouseButton.Right => _currentMouseEntry.RightButton == ButtonState.Pressed,
            MouseButton.Middle => _currentMouseEntry.MiddleButton == ButtonState.Pressed,
            _ => false
        };

        public static bool IsMouseButtonPressed(MouseButton button) => button switch
        {
            MouseButton.Left => _currentMouseEntry.LeftButton == ButtonState.Pressed && _prevMouseEntry.LeftButton == ButtonState.Released,
            MouseButton.Right => _currentMouseEntry.RightButton == ButtonState.Pressed && _prevMouseEntry.RightButton == ButtonState.Released,
            MouseButton.Middle => _currentMouseEntry.MiddleButton == ButtonState.Pressed && _prevMouseEntry.MiddleButton == ButtonState.Released,
            _ => false
        };
    }

    public enum MouseButton { Left, Right, Middle }
}