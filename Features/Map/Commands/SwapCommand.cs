using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TheDragonsPuzzleSeals.Features.Map
{
    public class SwapCommand(MapContextModel ctx, Seal seal, Vector2 distance) : ICommand
    {
        private readonly MapContextModel _ctx = ctx;
        private readonly Vector2 _distance = distance;
        private readonly Seal _seal = seal;
        private Dictionary<String, Vector2I> _dataSwap;

        public async Task ExecuteAync()
        {
            _dataSwap = SwapSystem.CalcData(_seal, _distance);

            await PlaySwap(_dataSwap["SwapFrom"], _dataSwap["SwapTo"]);
        }

        public async Task Undo()
        {
            await PlaySwap(_dataSwap["SwapTo"], _dataSwap["SwapFrom"]);
        }

        public async Task PlaySwap(Vector2I target1, Vector2I target2)
        {
            Seal seal1 = _ctx.SealViews[target1];
            Seal seal2 = _ctx.SealViews[target2];

            seal1.Model.MoveTo = _ctx.ConvertPosition(target2.X, target2.Y);
            seal2.Model.MoveTo = _ctx.ConvertPosition(target1.X, target1.Y);

            await MapAnimService.PlaySwap(seal1, seal2);
            
            SwapSystem.SwapData(_ctx.SealViews, target1, target2);
        }
    }
}
