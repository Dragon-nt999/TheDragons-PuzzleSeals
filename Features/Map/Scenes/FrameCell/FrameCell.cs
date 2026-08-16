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

        public FrameSealModel SetUp(SealModel model, Vector2 pos, 
                                    float sealSize, int width, 
                                    int height)
        {
            if (model == null) return null;

            Vector2 textureSize = _sprite.Texture.GetSize();

            int newSize = Convert.ToInt32(Math.Round(sealSize + (sealSize * distanceScale)));

            Vector2 scale = new Vector2(newSize / textureSize.X, newSize / textureSize.Y);
            _sprite.Scale = scale;

            string type = "center";

            Vector2 topleft  = new Vector2(distancePos, distancePos);
            Vector2 botleft  = new Vector2(distancePos, -distancePos);
            Vector2 topright = new Vector2(-distancePos, distancePos);
            Vector2 botright = new Vector2(-distancePos, -distancePos);
            Vector2 top      = new Vector2(0, distancePos);
            Vector2 left     = new Vector2(distancePos, 0);
            Vector2 bot      = new Vector2(0, -distancePos);
            Vector2 right    = new Vector2(-distancePos, 0);

            int index = _frameIndex[_rand.Next(_frameIndex.Length)];

            if (model.X == 0 && model.Y == 0)
            {
                type = "topleft";
                pos -= topleft;
            }
            else if ((model.X > 0 && model.X < width - 1) && model.Y == 0)
            {
                type = "top_" + index;
                pos -= top;
            }
            else if (model.X == 0 && (model.Y > 0 && model.Y < height - 1))
            {
                type = "left_" + index;
                pos -= left;
            }
            else if (model.X == 0 && model.Y == height - 1)
            {
                type = "botleft";
                pos -= botleft;
            }
            else if ((model.X > 0 && model.X < width - 1) && model.Y == height - 1)
            {
                type = "bot_" + index;
                pos -= bot;
            }
            else if (model.X == width - 1 && model.Y == 0)
            {
                type = "topright";
                pos -= topright;
            }
            else if (model.X == width - 1 && model.Y == height - 1)
            {
                type = "botright";
                pos -= botright;
            }
            else if (model.X == width - 1 && (model.Y > 0 && model.Y < height - 1))
            {
                type = "right_" + index;
                pos -= right;
            }

            string texturePath = $"res://Assets/Textures/FrameCell/{type}.png";

            Config = new FrameSealModel(texturePath, pos);

            return Config;
        }
    }
}