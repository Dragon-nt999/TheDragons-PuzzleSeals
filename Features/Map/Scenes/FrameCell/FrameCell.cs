using Godot;
using System;
using TheDragonsPuzzleSeals.Core.Managers;

namespace TheDragonsPuzzleSeals.Features.Map
{
    public partial class FrameCell : Node2D
    {
        private Sprite2D _sprite;
        private const float distanceScale = 0.05f;
        private const int distancePos = 6;
        private readonly FrameCellType[] _bot = [
            FrameCellType.bot_1,
            FrameCellType.bot_2,
            FrameCellType.bot_3,
            FrameCellType.bot_4,
            FrameCellType.bot_5,
        ];

        private readonly FrameCellType[] _left = [
            FrameCellType.left_1,
            FrameCellType.left_2,
            FrameCellType.left_3,
            FrameCellType.left_4,
            FrameCellType.left_5,
        ];

        private readonly FrameCellType[] _right = [
            FrameCellType.right_1,
            FrameCellType.right_2,
            FrameCellType.right_3,
            FrameCellType.right_4,
            FrameCellType.right_5,
        ];

        private readonly FrameCellType[] _top = [
            FrameCellType.top_1,
            FrameCellType.top_2,
            FrameCellType.top_3,
            FrameCellType.top_4,
            FrameCellType.top_5,
        ];

        public FrameSealModel Config = null;

        private Random _rand;
        public override void _Ready()
        {
            _sprite = GetNode<Sprite2D>("Object");
            _rand = new Random();
        }

        public void Initialize()
        {
            if (Config == null) return;
            _sprite.Texture = Config.Texture;
        }

        public FrameSealModel SetUp(Vector2I posCell, Vector2 pos, 
                                    float sealSize, int width, 
                                    int height)
        {
            Vector2 textureSize = _sprite.Texture.GetSize();

            int newSize = Convert.ToInt32(Math.Round(sealSize + (sealSize * distanceScale)));

            Vector2 scale = new(newSize / textureSize.X, newSize / textureSize.Y);
            _sprite.Scale = scale;

            Vector2 topleft  = new(distancePos, distancePos);
            Vector2 botleft  = new(distancePos, -distancePos);
            Vector2 topright = new(-distancePos, distancePos);
            Vector2 botright = new(-distancePos, -distancePos);
            Vector2 top      = new(0, distancePos);
            Vector2 left     = new(distancePos, 0);
            Vector2 bot      = new(0, -distancePos);
            Vector2 right    = new(-distancePos, 0);

            Texture2D texture = TextureManager.Instance.GetFrameCellTexture(FrameCellType.center);

            Texture2D botTexture = TextureManager.Instance.GetFrameCellTexture(_bot[_rand.Next(_bot.Length)]);
            Texture2D topTexture = TextureManager.Instance.GetFrameCellTexture(_top[_rand.Next(_top.Length)]);
            Texture2D leftTexture = TextureManager.Instance.GetFrameCellTexture(_left[_rand.Next(_left.Length)]);
            Texture2D rightTexture = TextureManager.Instance.GetFrameCellTexture(_right[_rand.Next(_right.Length)]);

            if (posCell.X == 0 && posCell.Y == 0)
            {
                texture = TextureManager.Instance.GetFrameCellTexture(FrameCellType.top_left);
                pos -= topleft;
            }
            else if (posCell.X > 0 && posCell.X < width - 1 && posCell.Y == 0)
            {
                texture = topTexture;
                pos -= top;
            }
            else if (posCell.X == 0 && posCell.Y > 0 && posCell.Y < height - 1)
            {
                texture = leftTexture;
                pos -= left;
            }
            else if (posCell.X == 0 && posCell.Y == height - 1)
            {
                texture = TextureManager.Instance.GetFrameCellTexture(FrameCellType.bot_left);
                pos -= botleft;
            }
            else if (posCell.X > 0 && posCell.X < width - 1 && posCell.Y == height - 1)
            {
                texture = botTexture;
                pos -= bot;
            }
            else if (posCell.X == width - 1 && posCell.Y == 0)
            {
                texture = TextureManager.Instance.GetFrameCellTexture(FrameCellType.top_right);
                pos -= topright;
            }
            else if (posCell.X == width - 1 && posCell.Y == height - 1)
            {
                texture = TextureManager.Instance.GetFrameCellTexture(FrameCellType.bot_right);
                pos -= botright;
            }
            else if (posCell.X == width - 1 && posCell.Y > 0 && posCell.Y < height - 1)
            {
                texture = rightTexture;
                pos -= right;
            }
            Config = new(texture, pos);

            return Config;
        }
    }
}