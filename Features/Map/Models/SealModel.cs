using Godot;
using System;

namespace TheDragonsPuzzleSeals.Features.Map
{
    public class SealModel(int x, int y, SealType type)
    {
        public int X { get; set; } = x;
        public int Y { get; set; } = y;
        public SealType Type { get; } = type;
        public SealAction? Action { get; set; } = null;
    }

}
