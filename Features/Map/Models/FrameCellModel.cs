using Godot;
using System;

namespace TheDragonsPuzzleSeals.Features.Map
{
    public class FrameSealModel(string texturePath, Vector2 pos)
    {
        public string TexturePath { get; } = texturePath;
        public Vector2 Position { get; } = pos;
    }

}