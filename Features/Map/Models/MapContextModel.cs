using Godot;
using System;
using System.Collections.Generic;

namespace TheDragonsPuzzleSeals.Features.Map
{
    public class MapContextModel
    {
        public Node2D Node { get; set; }
        public PackedScene SealScene { get; init; }
        public PackedScene StoneCellScene { get; init; }
        public PackedScene FrameCellScene { get; init; }
        public MapObjectModel[,] MapData { get; init; }
        public float SealSize { get; init; }
        public int Width { get; init; }
        public int Height { get; init; }
        public Func<int, int, Vector2> ConvertPosition { get; init; }
        public Dictionary<Vector2I, Seal> SealViews = [];
    }

}
