using Godot;
using System;
using TheDragonsPuzzleSeals.Core.Events;

namespace TheDragonsPuzzleSeals.Features.Map
{
    public partial class StoneCell : Node2D
    {
        private Sprite2D _sprite;
        private int[] _shapes = { 1, 2, 3, 4 };
        private Random _rand;
        private Vector2I _position;
        public override void _Ready()
        {
            _sprite = GetNode<Sprite2D>("Object");

            GameEventBus.Instance.SwapExecuted += OnSwapExecuted;
        }

        public void Initialize(Vector2I posCell, float size)
        {
            Vector2 textureSize = _sprite.Texture.GetSize();
            Vector2 scale       = new (size / textureSize.X, size / textureSize.Y);
            _sprite.Scale       = scale;
            _position           = posCell;

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
        
        public override void _ExitTree()
        {
            GameEventBus.Instance.SwapExecuted -= OnSwapExecuted;
        }

        private void OnSwapExecuted(SwapExecutedEvent @event)
        {
            if(_position == @event.Target1 || _position == @event.Target2)
            {
                Tween tween = this.CreateTween();

                tween.TweenProperty(_sprite, 
                                "self_modulate",
                                new Color(1.5f, 1.5f, 1.5f, 1f),
                                0.1f
                                );

                tween.TweenProperty(_sprite,
                                    "self_modulate",
                                    Colors.White,
                                    0.1f
                                    );
            }
        }
    }
}
