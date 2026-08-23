using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace TheDragonsPuzzleSeals.Features.Map
{
    public class SwapCommand(MapContextModel ctx, Vector2I from, Vector2I to)
    {
        private readonly MapContextModel _ctx = ctx;
        private readonly Vector2I _from = from;
        private readonly Vector2I _to = to;

        public async Task ExecuteAync()
        {
            //Seal seal1 = _ctx.SealViews[from];
            //Seal seal2 = _ctx.SealViews[to];
            //Vector2 target1 = _ctx.ConvertPosition(from.X, from.Y);
            //Vector2 target2 = _ctx.ConvertPosition(to.X, to.Y);

            //seal1.Model.Action = SealAction.Swap;
            //seal2.Model.Action = SealAction.Swap;

            //SwapSeal(from, to);

            //await MapAnimService.PlaySwap(seal1, seal2, target1, target2);

            await PlaySwap(_from, _to);
        }

        public async Task Undo()
        {
            /*Seal seal1 = _ctx.SealViews[oldCurrent];
            Seal seal2 = _ctx.SealViews[oldTarget];
            Vector2 target1 = _ctx.ConvertPosition(oldCurrent.X, oldCurrent.Y);
            Vector2 target2 = _ctx.ConvertPosition(oldTarget.X, oldTarget.Y);

            seal1.Model.Action = SealAction.Swap;
            seal2.Model.Action = SealAction.Swap;

            SwapSeal(oldCurrent, oldTarget);

            await MapAnimService.PlaySwap(seal1, seal2, target1, target2);*/
            await PlaySwap(_to, _from);
        }

        private async Task PlaySwap(Vector2I from, Vector2I to)
        {
            Seal seal1 = _ctx.SealViews[from];
            Seal seal2 = _ctx.SealViews[to];
            Vector2 target1 = _ctx.ConvertPosition(from.X, from.Y);
            Vector2 target2 = _ctx.ConvertPosition(to.X, to.Y);

            seal1.Model.Action = SealAction.Swap;
            seal2.Model.Action = SealAction.Swap;

            await MapAnimService.PlaySwap(seal1, seal2, target1, target2);

            SwapSeal(from, to);
        }

        private void SwapSeal(Vector2I current, Vector2I target)
        {

            // Update seal' model
            if (GodotObject.IsInstanceValid(_ctx.SealViews[current]))
            {
                _ctx.SealViews[current].Model.X = target.X;
                _ctx.SealViews[current].Model.Y = target.Y;
            }

            if (GodotObject.IsInstanceValid(_ctx.SealViews[target]))
            {
                _ctx.SealViews[target].Model.X = current.X;
                _ctx.SealViews[target].Model.Y = current.Y;
            }


            (_ctx.SealViews[current], _ctx.SealViews[target]) =
                    (_ctx.SealViews[target], _ctx.SealViews[current]);

        }
    }
}
