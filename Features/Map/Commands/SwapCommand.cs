using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace TheDragonsPuzzleSeals.Features.Map
{
    public class SwapCommand(MapContextModel ctx)
    {
        private readonly MapContextModel _ctx = ctx;
        private Vector2I oldCurrent;
        private Vector2I oldTarget;

        public async Task ExecuteAync()
        {
            if(_ctx.SwapData.Count == 2)
            {
                Vector2I current = _ctx.SwapData["current"];
                Vector2I target = _ctx.SwapData["target"];

                oldCurrent = current;
                oldTarget = target;

                Seal seal1 = _ctx.SealViews[current];
                Seal seal2 = _ctx.SealViews[target];
                Vector2 target1 = _ctx.ConvertPosition(current.X, current.Y);
                Vector2 target2 = _ctx.ConvertPosition(target.X, target.Y);

                await MapAnimService.PlaySwap(seal1, seal2, target1, target2);

                SwapSeal(current, target);

                _ctx.SwapData.Clear();
            }
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
