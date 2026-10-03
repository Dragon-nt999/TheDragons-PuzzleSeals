using Godot;
using System;
using TheDragonsPuzzleSeals.Core.Events;
using TheDragonsPuzzleSeals.Core.Managers;

namespace TheDragonsPuzzleSeals.Features.Map
{
    public partial class StoneCell : Node2D
    {
        private Sprite2D _sprite;
        private readonly StoneCellType[] _gray = [
            StoneCellType.cell_gray, 
            StoneCellType.cell_gray_1,
            StoneCellType.cell_gray_2,
            StoneCellType.cell_gray_3,
            StoneCellType.cell_gray_4
        ];

        private readonly StoneCellType[] _green = [
            StoneCellType.cell_green, 
            StoneCellType.cell_green_1,
            StoneCellType.cell_green_2,
            StoneCellType.cell_green_3,
            StoneCellType.cell_green_4
        ];

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
            StoneCellType gray = _gray[_rand.Next(_gray.Length)];

            _sprite.Texture = TextureManager.Instance.GetStoneCellTexture(gray);

            if((posCell.Y % 2 == 0 && posCell.X % 2 == 0) || (posCell.Y % 2 != 0 && posCell.X % 2 != 0))
            {
                StoneCellType green = _green[_rand.Next(_green.Length)];
                _sprite.Texture = TextureManager.Instance.GetStoneCellTexture(green);
            }
        }
        
        public override void _ExitTree()
        {
            GameEventBus.Instance.SwapExecuted -= OnSwapExecuted;
        }

        private void OnSwapExecuted(SwapExecutedEvent @event)
        {
            if(_position == @event.Target)
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
