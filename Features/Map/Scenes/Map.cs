using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
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

        private MapObjectModel[,] _mapData;

        private Seal _seletedSeal;
        private Vector2 _startPostion;

        private MapRenderService _renderService;

        private const float SwipeThreshold = 35.0f;

        private MapContextModel _ctx; 

        public override async void _Ready()
        {
            _mapArea = GetParent<Control>();
            this.Position = Vector2.Zero;

            // Wait one frame for the parent container to calculate its actual size
            await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

            // Calculate Seal size, offset
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

            // Create Map
            CreateMap();

            if (_mapData == null || _mapData.Length <= 0) return;

            // Initial Map Contex for Render, Animation, Commands
            _ctx = new MapContextModel
            {
                Node            = this,
                SealScene       = SealScene,
                StoneCellScene  = StoneCellScene,
                FrameCellScene  = FrameCellScene,
                MapData         = _mapData,
                SealSize        = _sealSize,
                Width           = _width,
                Height          = _height,
                ConvertPosition = ConvertPostion
            };

            // Render
            _renderService = new MapRenderService(_ctx);
            if(_renderService != null)
            {
                _renderService.Render();
                _renderService.SealTouched += OnSealTouched;
            }

        }

        /// <summary>
        /// Calculate position Seal or somethings else from SealMode[X, Y]
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private Vector2 ConvertPostion(int x, int y)
        {
            float xPos = x * _sealSize + _offsetX + (_sealSize / 2);
            float yPos = y * _sealSize + _offsetY + (_sealSize / 2);

            return new Vector2(xPos, yPos);
        }

        /// <summary>
        /// Helper for assign seal, postion from Seal's Signal
        /// </summary>
        /// <param name="seal"></param>
        /// <param name="startPosition"></param>
        private void OnSealTouched(Seal seal, Vector2 startPosition)
        {
            _seletedSeal = seal;
            _startPostion = startPosition;
        }

        /// <summary>
        /// Handle click or touch events
        /// get seal, position from Seal's Signal
        /// </summary>
        /// <param name="event"></param>
        public override async void _UnhandledInput(InputEvent @event)
        {
            if(@event is InputEventMouseButton mouseButton
                            && mouseButton.ButtonIndex == MouseButton.Left)
            {
                if(!mouseButton.Pressed && _seletedSeal != null)
                {
                    await HandleSwipe(mouseButton.Position);
                }
            }  
        }

        private async Task HandleSwipe(Vector2 endPosition)
        {
            Vector2 distance = endPosition - _startPostion;
            if(distance.Length() < SwipeThreshold)
            {
                _seletedSeal = null;
                return;
            }

            if ((_seletedSeal.Model.X >= 0 && _seletedSeal.Model.X < _width)
                    && (_seletedSeal.Model.Y >= 0 && _seletedSeal.Model.Y < _height))
            {
                int currentX = _seletedSeal.Model.X;
                int currentY = _seletedSeal.Model.Y;
                int targetX = currentX;
                int targetY = currentY;

                if (MathF.Abs(distance.X) > MathF.Abs(distance.Y))
                {
                    targetX += distance.X > 0 ? 1 : -1;
                }
                else
                {
                    targetY += distance.Y > 0 ? 1 : -1;
                }

                var swapFrom = new Vector2I(currentX, currentY);
                var swapTo = new Vector2I(targetX, targetY);

                SwapCommand swapCommand = new SwapCommand(_ctx, swapFrom, swapTo);
                await swapCommand.ExecuteAync();

                // Find matches
                Dictionary<SealType, HashSet<SealModel>> matches = MatchSystem.findMatch(_ctx);

                if(matches.Count > 0)
                {

                } else
                {
                    await swapCommand.Undo();
                }


                // Reset Swap
                _seletedSeal = null;
            }
            

        }

        /// <summary>
        /// Create Map base on widh, height
        /// </summary>
        private void CreateMap()
        {
            _mapData = new MapObjectModel[_width, _height];
            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    _mapData[x, y] = new MapObjectModel(x, y);
                }
            }
        }

        public override void _ExitTree()
        {
            if(_renderService != null)
            {
                _renderService.SealTouched -= OnSealTouched;
                _renderService.Clear();
            }
        }
    }
}

