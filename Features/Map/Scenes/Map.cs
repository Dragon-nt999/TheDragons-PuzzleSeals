using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheDragonsPuzzleSeals.Core.Managers;

namespace TheDragonsPuzzleSeals.Features.Map
{
    public partial class Map : Node2D
    {
        [Export] public PackedScene SealScene { get; set; }
        [Export] public PackedScene StoneCellScene { get; set; }
        [Export] public PackedScene FrameCellScene { get; set; }

        private Control _mapArea;

        private readonly int _width        = 9;
        private readonly int _height       = 11;
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
        private bool _isResolvingMatch = false;

        public override async void _Ready()
        {
            _mapArea = GetParent<Control>();
            this.Position = Vector2.Zero;

            // Avoid lag for first time swap seal
            WarmupTweenEngine();

            // Wait one frame for the parent container to calculate its actual size
            await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

            // Load VFX
            VfxManager.Instance.ClearCache();
            await VfxManager.Instance.PreloadAndWarmupVfx(VfxType.Explosion);
            
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
                OffsetX         = _offsetX,
                OffsetY         = _offsetY,
                Width           = _width,
                Height          = _height,
                OnSealTouched   = OnSealTouched,
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
            if(_isResolvingMatch) return;

            Vector2 distance = endPosition - _startPostion;
            if(distance.Length() < SwipeThreshold)
            {
                _seletedSeal = null;
                return;
            }

            if (_seletedSeal.Model.X >= 0 && _seletedSeal.Model.X < _width
                    && _seletedSeal.Model.Y >= 0 && _seletedSeal.Model.Y < _height)
            {
                try
                {
                    // Swap seals
                    SwapCommand swapCommand = new(_ctx, _seletedSeal, distance);
                    await swapCommand.ExecuteAync();
                    
                    // Finding matches
                    List<HashSet<Seal>> matches = MatchSystem.FindAndGroupMatch(_ctx);

                    // Processing matches
                    if(matches.Count > 0)
                    {
                        _isResolvingMatch = true;
                        await new ResolveMatchCommand(_ctx, matches).ExecuteAync();
                    } else
                    {
                        await swapCommand.Undo();
                    }
                } 
                finally
                {
                    // Reset Swap
                    _seletedSeal = null;
                    _isResolvingMatch = false;
                }
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

        private void ClearMap()
        {
            if(_mapData != null && _mapData.Length > 0)
            {
                for (var x = 0; x < _width; x++)
                {
                    for (var y = 0; y < _height; y++)
                    {
                        _mapData[x, y] = null;
                    }
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


        private void WarmupTweenEngine()
        {
            var warmup = CreateTween();
            warmup.TweenProperty(this, CanvasItem.PropertyName.SelfModulate.ToString(), Colors.White, 0.001f);
            warmup.TweenProperty(this, Node2D.PropertyName.Scale.ToString(), Vector2.One, 0.001f);
            warmup.TweenProperty(this, Node2D.PropertyName.Position.ToString(), Position, 0.001f);
            warmup.CustomStep(0.001f);
            warmup.Kill();
        }
    }
}

