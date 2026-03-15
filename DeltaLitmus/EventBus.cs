namespace DeltaLitmus {
    public delegate void RefAction<T>(ref T data) where T : struct;

    public static class EventBus<T> where T : struct {
        private static RefAction<T> _listeners;

        public static void Subscribe(RefAction<T> listener) => _listeners += listener;
        public static void Unsubscribe(RefAction<T> listener) => _listeners -= listener;
        public static void Raise(ref T eventData) => _listeners?.Invoke(ref eventData);
    }
}