namespace DeltaLitmus.Systems.Core {
    public static class ComponentMeta<T> where T : struct
    {
        public static readonly int Id = ComponentMetaCounter.Next();
    }

    internal static class ComponentMetaCounter
    {
        private static int _counter = 0;
        public static int Next() => _counter++;
    }
}