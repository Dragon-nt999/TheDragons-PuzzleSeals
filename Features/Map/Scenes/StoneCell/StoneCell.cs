using Godot;
using System;

namespace TheDragonsPuzzleSeals.Features.Map
{
    public partial class StoneCell : Node2D
    {
        private Sprite2D _sprite;
        private int[] _shapes = { 1, 2, 3, 4 };
        private Random _rand;
        public override void _Ready()
        {
            _sprite = GetNode<Sprite2D>("Object");
        }

        public void Initialize(Vector2I posCell, float size)
        {
            Vector2 textureSize = _sprite.Texture.GetSize();
            Vector2 scale = new Vector2(size / textureSize.X, size / textureSize.Y);
            _sprite.Scale = scale;

            _rand = new Random();
            int shape = _shapes[_rand.Next(_shapes.Length)];

            string texturePath = $"res://Assets/Textures/StoneCell/cell_green_{shape}.png";

            if((posCell.Y % 2 == 0 && posCell.X % 2 == 0) || (posCell.Y % 2 != 0 && posCell.X % 2 != 0))
            {
                texturePath = $"res://Assets/Textures/StoneCell/cell_gray_{shape}.png";
            }

            try
            {
                _sprite.Texture = GD.Load<Texture2D>(texturePath);
            }
            catch (Exception e)
            {
                GD.Print("Error when loading texture " + e.Message);
            }
        }
    }
}
