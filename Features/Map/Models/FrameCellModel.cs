using Godot;
using System;

namespace TheDragonsPuzzleSeals.Features.Map
{
    public class FrameSealModel
    {
        public Vector2 Position;
        public string TexturePath;

        public FrameSealModel(string texturePath, Vector2 pos)
        {
            TexturePath = texturePath;
            Position = pos;
        }
    }

}