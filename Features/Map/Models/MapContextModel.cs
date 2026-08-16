using Godot;
using System;

namespace TheDragonsPuzzleSeals.Features.Map
{
    public class MapContextModel
    {
        public Node2D Node { get; set; }
        public PackedScene SealScene { get; init; }
        public PackedScene StoneCellScene { get; init; }
        public PackedScene FrameCellScene { get; init; }
        public SealModel[,] MapData { get; init; }
        public Seal[,] SealData { get; set; }
        public float SealSize { get; init; }
        public int Width { get; init; }
        public int Height { get; init; }
        public Func<int, int, Vector2> ConvertPosition { get; init; }
    }

}
