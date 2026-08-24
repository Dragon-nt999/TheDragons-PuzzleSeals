using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace TheDragonsPuzzleSeals.Features.Map
{
    public class SwapCommand(MapContextModel ctx, Vector2I from, Vector2I to) : ICommand
    {
        private readonly MapContextModel _ctx = ctx;
        private readonly Vector2I _from = from;
        private readonly Vector2I _to = to;

        public async Task ExecuteAync()
        {
            await PlaySwap(_from, _to);
        }

        public async Task Undo()
        {
            await PlaySwap(_to, _from);
        }

        private async Task PlaySwap(Vector2I from, Vector2I to, bool undo = false)
        {
            Seal seal1 = _ctx.SealViews[from];
            Seal seal2 = _ctx.SealViews[to];
            Vector2 target1 = _ctx.ConvertPosition(from.X, from.Y);
            Vector2 target2 = _ctx.ConvertPosition(to.X, to.Y);

            if(undo == false)
            {
                seal1.Model.Action = SealAction.Swap;
                seal2.Model.Action = SealAction.Swap;
            } else
            {
                seal1.Model.Action = null;
                seal2.Model.Action = null;
            }
            
            await MapAnimService.PlaySwap(seal1, seal2, target1, target2);

            _ctx.SwapData(from, to);
        }
    }
}
