using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Pathfinding
{
    public struct TileInfo
    {
        public Vector3 Position;
        public Vector2Int PositionInGrid;
        public List<Vector2Int> Neighbours;

        public TileInfo(Vector3 position, Vector2Int positionInGrid, List<Vector2Int> neighbours)
        {
            Position = position;
            PositionInGrid = positionInGrid;
            Neighbours = neighbours;
        }
    }
}