using Microsoft.Xna.Framework.Input;
using System;

namespace DeltaLitmus.Core.InputSystem {
    public readonly struct Key {
        public readonly Keys KeyValue;
        public readonly KeyType KeyType;
        public readonly Action KeyAction;

        public Key(Keys keyValue, KeyType keyType, Action keyAction) {
            KeyValue = keyValue;
            KeyType = keyType;
            KeyAction = keyAction;
        }
    }

    public enum KeyType {
        Pressed,
        Released,
        Held,
    };
}