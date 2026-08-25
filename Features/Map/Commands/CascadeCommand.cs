using Godot;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheDragonsPuzzleSeals.Features.Map
{
    public class CascadeCommand(MapContextModel ctx) : ICommand
    {
        private readonly MapContextModel _ctx = ctx;
        public async Task ExecuteAync()
        {
            List<Seal> moves = CalculateCascade();
            if(moves.Count > 0)
            {
               await MapAnimService.PlayCascade(moves);
            }
        }

        private List<Seal> CalculateCascade()
        {
            List<Seal> moves = [];

            var cellsEmpty = FindAndSortCellEmpty();

            foreach (var model in cellsEmpty)
            {
                int toIndex = model.Y;

                for(int fromIndex = model.Y; fromIndex >= 0; fromIndex--)
                {
                    var from = new Vector2I(model.X, fromIndex);

                    if(GodotObject.IsInstanceValid(_ctx.SealViews[from]))
                    {
                        var to = new Vector2I(model.X, toIndex);

                        Seal seal = _ctx.SealViews[from];
                        seal.Model.Action = SealAction.Fall;
                        seal.Model.MoveTo = _ctx.ConvertPosition(to.X, to.Y);

                        moves.Add(seal);

                        _ctx.SwapData(from, to);

                        toIndex--;
                    }
                }
            }

            return moves;
        }

        private List<MapObjectModel> FindAndSortCellEmpty()
        {
            List<MapObjectModel> data = [];

            foreach(var cell in _ctx.MapData)
            {
                if(cell.Type == null)
                {
                    data.Add(cell);
                }
            }

            List<MapObjectModel> distinct = [.. data.OrderByDescending(arr => arr.Y)
                .GroupBy(arr => arr.X)
                .Select(g => g.First())];

            return distinct;
        }
    }

}
