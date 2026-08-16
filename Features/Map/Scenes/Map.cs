using Godot;
using System;
using System.Threading.Tasks;

namespace TheDragonsPuzzleSeals.Features.Map
{
    public partial class Map : Node2D
    {
        [Export] public PackedScene SealScene { get; set; }
        [Export] public PackedScene StoneCellScene { get; set; }
        [Export] public PackedScene FrameCellScene { get; set; }

        private Control _mapArea;

        private readonly int _width = 9;
        private readonly int _height = 11;
        private readonly float maxSealSize = 116;
        private float _sealSize;

        private float _offsetX;
        private float _offsetY;

        private SealModel[,] _mapData;
        private Seal[,] _sealData;

        public override async void _Ready()
        {
            _mapArea = GetParent<Control>();
            this.Position = Vector2.Zero;

            // Wait one frame for the parent container to calculate its actual size
            await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

            Vector2 mapSize = _mapArea.Size;

            _sealSize = Mathf.Floor(Mathf.Min(
                    mapSize.X / _width,
                    mapSize.Y / _height
                ));

            if(_sealSize > maxSealSize)
            {
                _sealSize = maxSealSize;
            }

            float mapWidth  = _width * _sealSize;
            float mapHeight = _height * _sealSize;

            _offsetX = (mapSize.X - mapWidth) / 2f;
            _offsetY = (mapSize.Y - mapHeight) / 2f;

            _mapData = SpawnSystem.GenerateMap(_width, _height);
            _sealData = new Seal[_width, _height];

            if (_mapData == null) return;

            MapContextModel ctx = new MapContextModel
            {
                Node            = this,
                SealScene       = SealScene,
                StoneCellScene  = StoneCellScene,
                FrameCellScene  = FrameCellScene,
                MapData         = _mapData,
                SealSize        = _sealSize,
                Width           = _width,
                Height          = _height,
                ConvertPosition = converPostion,
                SealData        = _sealData,
            };

            // Render
            MapRenderService render = new MapRenderService(ctx);
            render.Render();

            // Command

        }

        private Vector2 converPostion(int x, int y)
        {
            float xPos = x * _sealSize + _offsetX + (_sealSize / 2);
            float yPos = y * _sealSize + _offsetY + (_sealSize / 2);

            return new Vector2(xPos, yPos);
        }
    }
}

