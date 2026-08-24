using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TheDragonsPuzzleSeals.Features.Map
{
    public class CascadeCommand(MapContextModel ctx, HashSet<SealModel> matches) : ICommand
    {
        private readonly MapContextModel _ctx = ctx;
        private readonly HashSet<SealModel> _matches = matches;
        public async Task ExecuteAync()
        {
            List<MoveModel> moves = CalculateCascade();
            if(moves.Count > 0)
            {
                await MapAnimService.PlayCascade(moves);
            }
        }

        private List<MoveModel> CalculateCascade()
        {
            List<MoveModel> moves = [];

            foreach(var model in _matches)
            {
                for(int y = model.Y; y > 0; y--)
                {
                    var x = model.X;
                    var fromIndex = new Vector2I(x, y - 1);
                    var toIndex = new Vector2I(x, y);
                    if (GodotObject.IsInstanceValid(_ctx.SealViews[fromIndex]))
                    {
                        Vector2 from = _ctx.ConvertPosition(fromIndex.X, fromIndex.Y);
                        Vector2 to   = _ctx.ConvertPosition(toIndex.X, toIndex.Y);
                        Seal seal    = _ctx.SealViews[fromIndex];
                        seal.Model.Action = SealAction.Fall;

                        MoveModel cascadeSeal = new(from, to, seal);
                        moves.Add(cascadeSeal);

                        _ctx.SwapData(fromIndex, toIndex);
                    }
                }
            }

            return moves;
        }
    }

}
