using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Pathfinding
{
    public class TileInfo
    {
        public Vector3 Position { get; private set; }
        public Vector2Int PositionInGrid { get; private set; }
        public List<Vector2Int> Neighbours { get; private set; }

        public TileInfo(Vector3 position, Vector2Int positionInGrid, List<Vector2Int> neighbours)
        {
            Position = position;
            PositionInGrid = positionInGrid;
            Neighbours = neighbours;
        }
    }
}