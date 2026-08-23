using Godot;
using System;

namespace TheDragonsPuzzleSeals.Features.Map
{
    public partial class FrameCell : Node2D
    {
        private Sprite2D _sprite;
        private const float distanceScale = 0.05f;
        private const int distancePos = 6;

        private readonly int[] _frameIndex = { 1, 2, 3, 4 };

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

            try
            {
                _sprite.Texture = GD.Load<Texture2D>(Config.TexturePath);
            }
            catch (Exception e)
            {
                GD.Print("Error when loading texture " + e.Message);
            }
        }

        public FrameSealModel SetUp(Vector2I posCell, Vector2 pos, 
                                    float sealSize, int width, 
                                    int height)
        {
            Vector2 textureSize = _sprite.Texture.GetSize();

            int newSize = Convert.ToInt32(Math.Round(sealSize + (sealSize * distanceScale)));

            Vector2 scale = new(newSize / textureSize.X, newSize / textureSize.Y);
            _sprite.Scale = scale;

            string type = "center";

            Vector2 topleft  = new(distancePos, distancePos);
            Vector2 botleft  = new(distancePos, -distancePos);
            Vector2 topright = new(-distancePos, distancePos);
            Vector2 botright = new(-distancePos, -distancePos);
            Vector2 top      = new(0, distancePos);
            Vector2 left     = new(distancePos, 0);
            Vector2 bot      = new(0, -distancePos);
            Vector2 right    = new(-distancePos, 0);

            int index = _frameIndex[_rand.Next(_frameIndex.Length)];

            if (posCell.X == 0 && posCell.Y == 0)
            {
                type = "topleft";
                pos -= topleft;
            }
            else if ((posCell.X > 0 && posCell.X < width - 1) && posCell.Y == 0)
            {
                type = "top_" + index;
                pos -= top;
            }
            else if (posCell.X == 0 && (posCell.Y > 0 && posCell.Y < height - 1))
            {
                type = "left_" + index;
                pos -= left;
            }
            else if (posCell.X == 0 && posCell.Y == height - 1)
            {
                type = "botleft";
                pos -= botleft;
            }
            else if ((posCell.X > 0 && posCell.X < width - 1) && posCell.Y == height - 1)
            {
                type = "bot_" + index;
                pos -= bot;
            }
            else if (posCell.X == width - 1 && posCell.Y == 0)
            {
                type = "topright";
                pos -= topright;
            }
            else if (posCell.X == width - 1 && posCell.Y == height - 1)
            {
                type = "botright";
                pos -= botright;
            }
            else if (posCell.X == width - 1 && (posCell.Y > 0 && posCell.Y < height - 1))
            {
                type = "right_" + index;
                pos -= right;
            }

            string texturePath = $"res://Assets/Textures/FrameCell/{type}.png";

            Config = new(texturePath, pos);

            return Config;
        }
    }
}