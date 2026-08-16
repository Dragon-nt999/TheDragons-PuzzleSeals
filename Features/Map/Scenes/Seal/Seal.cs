using Godot;
using System;
namespace TheDragonsPuzzleSeals.Features.Map
{
    public partial class Seal : Area2D
    {
        private Sprite2D _sprite;
        public override void _Ready()
        {
            _sprite = GetNode<Sprite2D>("Object");
        }

        public void Initialize(SealModel model, float size)
        {
            if (model == null) return;

            Vector2 textureSize = _sprite.Texture.GetSize();
            Vector2 scale = new Vector2(size / textureSize.X, size / textureSize.Y);
            _sprite.Scale = scale;

            string texturePath = $"res://Assets/Textures/Seals/seal_{model.Type}.png";

            try
            {
                _sprite.Texture = GD.Load<Texture2D>(texturePath);
            } catch(Exception e)
            {
                GD.Print("Error when loading texture " + e.Message);
            }
        }
    }
}

