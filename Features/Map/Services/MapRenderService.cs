using Godot;
using System;
using TheDragonsPuzzleSeals.Features.Map;

namespace TheDragonsPuzzleSeals.Features.Map
{
    public class MapRenderService
    {
        private readonly MapContextModel _ctx;

        public MapRenderService(MapContextModel ctx)
        {
            _ctx = ctx;
        }

        public void Render()
        {
            foreach (var model in _ctx.MapData)
            {
                StoneCell(model);
                FrameCell(model);
                Seal(model);
            }
        }

        private void Seal(SealModel model)
        {
            if (model == null) return;

            Seal seal = _ctx.SealScene.Instantiate<Seal>();
            _ctx.Node.AddChild(seal);
            seal.Initialize(model, _ctx.SealSize);
            seal.Position = _ctx.ConvertPosition(model.X, model.Y);

            if(GodotObject.IsInstanceValid(seal))
            {
                _ctx.SealData[model.X, model.Y] = seal;
            }
        }

        private void StoneCell(SealModel model)
        {
            if (model == null) return;

            StoneCell cell = _ctx.StoneCellScene.Instantiate<StoneCell>();
            _ctx.Node.AddChild(cell);
            cell.Initialize(model, _ctx.SealSize);
            cell.Position = _ctx.ConvertPosition(model.X, model.Y);
        }

        private void FrameCell(SealModel model)
        {
            if (model == null) return;
            FrameCell frameCell = _ctx.FrameCellScene.Instantiate<FrameCell>();
            _ctx.Node.AddChild(frameCell);

            Vector2 pos = _ctx.ConvertPosition(model.X, model.Y);
            frameCell.SetUp(model, pos, _ctx.SealSize, _ctx.Width, _ctx.Height);
            frameCell.Initialize();
            frameCell.Position = frameCell.Config.Position;
        }
    }
}

