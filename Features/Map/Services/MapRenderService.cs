using Godot;
using System;
using System.Collections.Generic;

namespace TheDragonsPuzzleSeals.Features.Map
{
    public class MapRenderService(MapContextModel ctx)
    {
        private readonly MapContextModel _ctx = ctx;
        public event Action<Seal, Vector2> SealTouched;

        private readonly Dictionary<Vector2I, StoneCell> _stoneCellViews = [];
        private readonly Dictionary<Vector2I, FrameCell> _frameCellViews = [];

        /// <summary>
        /// Render Seal, Stone Cell, Frame Cell Base on SealModel[,]
        /// </summary>
        public void Render()
        {
            StoneCells();
            FrameCells();
            Seals();
        }

        /// <summary>
        /// Clear Map when close or leave game
        /// </summary>
        public void Clear()
        {
            // Clear Seal
            foreach (var t in _ctx.SealViews)
            {
                Seal seal = t.Value;
                if(GodotObject.IsInstanceValid(seal))
                {
                    seal.SealTouched -= OnSealTouched;
                    seal.QueueFree();
                }
            }
            _ctx.SealViews.Clear();

            // Clear Stone Cell
            foreach (var c in _stoneCellViews)
            {
                StoneCell cell = c.Value;
                if (GodotObject.IsInstanceValid(cell))
                {
                    cell.QueueFree();
                }
            }
            _stoneCellViews.Clear();

            // Clear Frame Cell
            foreach (var f in _frameCellViews)
            {
                FrameCell frame = f.Value;
                if (GodotObject.IsInstanceValid(frame))
                {
                    frame.QueueFree();
                }
            }
            _frameCellViews.Clear();

        }

        /// <summary>
        /// Render Seals on Map
        /// </summary>
        private void Seals()
        {
            Dictionary<Vector2I, SealModel> seals = SpawnSystem.SpawnSeals(_ctx.MapData);
            foreach(var (index, model) in seals)
            {
                Seal seal = _ctx.SealScene.Instantiate<Seal>();
                seal.SealTouched += OnSealTouched;

                _ctx.Node.AddChild(seal);
                seal.Initialize(model, _ctx.SealSize);

                seal.Position = _ctx.ConvertPosition(index.X, model.Y);

                if (GodotObject.IsInstanceValid(seal))
                {
                    _ctx.SealViews[index] = seal;
                }
            }
        }

        /// <summary>
        /// Render Cell on Map, which under Seal
        /// </summary>
        private void StoneCells()
        {
            foreach(var obj in _ctx.MapData)
            {
                StoneCell cell = _ctx.StoneCellScene.Instantiate<StoneCell>();
                _ctx.Node.AddChild(cell);
                Vector2I pos = new Vector2I(obj.X, obj.Y);
                cell.Initialize(pos, _ctx.SealSize);
                cell.Position = _ctx.ConvertPosition(pos.X, pos.Y);

                if (GodotObject.IsInstanceValid(cell))
                {
                    _stoneCellViews[pos] = cell;
                }
            }
        }


        /// <summary>
        /// Render Frames, which surround map
        /// </summary>
        private void FrameCells()
        {
            foreach (var obj in _ctx.MapData)
            {
                FrameCell frameCell = _ctx.FrameCellScene.Instantiate<FrameCell>();
                _ctx.Node.AddChild(frameCell);

                Vector2I index = new Vector2I(obj.X, obj.Y);

                Vector2 pos = _ctx.ConvertPosition(index.X, index.Y);
                frameCell.SetUp(index, pos, _ctx.SealSize, _ctx.Width, _ctx.Height);
                frameCell.Initialize();

                frameCell.Position = frameCell.Config.Position;

                if (GodotObject.IsInstanceValid(frameCell))
                {
                    _frameCellViews[index] = frameCell;
                }
            }
            
        }

        /// <summary>
        /// Event for Seal selected Input
        /// </summary>
        /// <param name="seal"></param>
        /// <param name="mousePosition"></param>
        private void OnSealTouched(Seal seal, Vector2 mousePosition)
        {
            SealTouched?.Invoke(seal, mousePosition);
        }
    }
}

