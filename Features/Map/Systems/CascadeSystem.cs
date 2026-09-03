using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheDragonsPuzzleSeals.Features.Map
{
    public class CascadeSystem (MapContextModel ctx, 
                                List<MapObjectModel> cellNull)
    {
        private readonly MapContextModel _ctx = ctx;
        private List<MapObjectModel> _cellNull = cellNull;

        public async Task PlayCascadeAsync()
        {
            var moveSeals = CalcCascade();
            await MapAnimService.PlayCascade(moveSeals);
        }

        private List<Seal> CalcCascade()
        {
            List<Seal> moves = [];

            List<MapObjectModel> cellsNull = [.. SortCellNull()];

            foreach (var model in cellsNull)
            {
                int toIndex = model.Y;

                for (int fromIndex = model.Y; fromIndex >= 0; fromIndex--)
                {
                    var from = new Vector2I(model.X, fromIndex);

                    if (GodotObject.IsInstanceValid(_ctx.SealViews[from]))
                    {
                        var to = new Vector2I(model.X, toIndex);

                        Seal seal = _ctx.SealViews[from];
                        seal.Model.X = to.X;
                        seal.Model.Y = to.Y;
                        seal.Model.MoveTo = _ctx.ConvertPosition(to.X, to.Y);
                        moves.Add(seal);

                        // Update cell on Map
                        _ctx.MapData[from.X, from.Y].Type = ObjectType.Null;
                        _ctx.MapData[to.X, to.Y].Type = ObjectType.Seal;

                        // Update seal data
                        (_ctx.SealViews[from], _ctx.SealViews[to]) = 
                                        (_ctx.SealViews[to], _ctx.SealViews[from]);

                        toIndex--;
                    }
                }
            }

            return moves;
        }

        private List<MapObjectModel> SortCellNull()
        {
            List<MapObjectModel> distinct = [.. _cellNull.OrderByDescending(arr => arr.Y)
                .GroupBy(arr => arr.X)
                .Select(g => g.First())];

            return distinct;
        }
    }

}
