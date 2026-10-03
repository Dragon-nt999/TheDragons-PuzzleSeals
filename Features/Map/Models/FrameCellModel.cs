using Godot;
using System;

namespace TheDragonsPuzzleSeals.Features.Map
{
    public class FrameSealModel(Texture2D texture, Vector2 pos)
    {
        public Texture2D Texture { get; } = texture;
        public Vector2 Position { get; } = pos;
    }

}