using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace DeltaLitmus.Systems.Core {
    public sealed class SpatialGrid
    {
        private readonly int _cellSize;
        private readonly Dictionary<long, List<int>> _cells = new();

        public SpatialGrid(int cellSize) => _cellSize = cellSize;

        public void Clear() => _cells.Clear();

        public void Insert(int id, Rectangle bounds)
        {
            int minX = bounds.Left / _cellSize;
            int maxX = bounds.Right / _cellSize;
            int minY = bounds.Top / _cellSize;
            int maxY = bounds.Bottom / _cellSize;

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    long key = ((long)x << 32) | (uint)y;
                    if (!_cells.TryGetValue(key, out var list))
                    {
                        list = new List<int>();
                        _cells[key] = list;
                    }
                    list.Add(id);
                }
            }
        }

        public IEnumerable<int> GetNearby(Rectangle bounds)
        {
            HashSet<int> nearby = new HashSet<int>();
            int minX = bounds.Left / _cellSize;
            int maxX = bounds.Right / _cellSize;
            int minY = bounds.Top / _cellSize;
            int maxY = bounds.Bottom / _cellSize;

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    long key = ((long)x << 32) | (uint)y;
                    if (_cells.TryGetValue(key, out var list))
                    {
                        foreach (var id in list) nearby.Add(id);
                    }
                }
            }
            return nearby;
        }
    }
}