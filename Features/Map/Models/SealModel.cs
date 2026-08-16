using Godot;
using System;

namespace TheDragonsPuzzleSeals.Features.Map
{
    public class SealModel
    {
        public int X;
        public int Y;
        public SealType Type;

        public SealModel(int x, int y, SealType type)
        {
            X    = x;
            Y    = y;
            Type = type;
        }
    }

}
