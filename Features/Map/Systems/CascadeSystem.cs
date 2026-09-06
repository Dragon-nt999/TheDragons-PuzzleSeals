using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheDragonsPuzzleSeals.Features.Map
{
    public class CascadeSystem (MapContextModel ctx)
    {
        private readonly MapContextModel _ctx = ctx;
        private readonly List<Seal> _sealsCascadeList = [];
        private Vector2I _emptyCellRemaining;
        private readonly MapRenderService render = new(ctx);

        public async Task PlayCascadeAsync()
        {
            // Calculate and get seals
            GetSealsToCascade();

            // Play animation cascade
            await MapAnimService.PlayCascade(_sealsCascadeList);

            // Reset Seals
            ResetSealsAfterCascade();
        }

        /// <summary>
        /// Calculate cascading tiles to fill empty cell
        /// from empty cell list
        /// </summary>
        private void GetSealsToCascade()
        {
            // Sorting and get empty cell 
            // which has highest Y property on per column
            List<MapObjectModel> cells = [.. SortAndGetEmptyCells()];

            foreach (var cell in cells)
            {
                GetSealsAboveEmptyCell(cell);
                FillEmptyCells();
            }

        }

        /// <summary>
        /// Calculate cascading seals to fill empty cell by one column
        /// </summary>
        /// <param name="cell"></param>
        private void GetSealsAboveEmptyCell(MapObjectModel cell)
        {
            int toIndex = cell.Y;
            for (int fromIndex = cell.Y; fromIndex >= 0; fromIndex--)
            {
                var from = new Vector2I(cell.X, fromIndex);
                var to = new Vector2I(cell.X, toIndex);

                if (GodotObject.IsInstanceValid(_ctx.SealViews[from]))
                {
                    Seal seal = _ctx.SealViews[from];
                    seal.Model.X = to.X;
                    seal.Model.Y = to.Y;
                    seal.Model.MoveTo = _ctx.ConvertPosition(to.X, to.Y);
                    _sealsCascadeList.Add(seal);

                    // Update type of the cell on map with type = null
                    _ctx.MapData[from.X, from.Y].Type = ObjectType.Null;

                    // Update type of the cell on map with type = seal
                    _ctx.MapData[to.X, to.Y].Type = ObjectType.Seal;

                    // Update seal's data on list of Seal views
                    (_ctx.SealViews[from], _ctx.SealViews[to]) = 
                                    (_ctx.SealViews[to], _ctx.SealViews[from]);
                    toIndex--;
                }

                // Contains the last empty cell on the column
                // after calculate cascade
                _emptyCellRemaining = to;
            }
        }

        /// <summary>
        /// Fill seal on remaining empty cells after cascading
        /// </summary>
        private void FillEmptyCells()
        {
            SealType[] poolType  =
                [
                    SealType.red,
                    SealType.blue,
                    SealType.green,
                    SealType.yellow,
                ];

            var rand = new Random();
            Dictionary<Vector2I, SealModel> modelList = [];

            for(int y = _emptyCellRemaining.Y; y >= 0; y--)
            {
                var cellEmpty = new Vector2I(_emptyCellRemaining.X, y);
                var cellEmptyAbove =  new Vector2I(_emptyCellRemaining.X, y - 1);
                if(!GodotObject.IsInstanceValid(_ctx.SealViews[cellEmpty]))
                {
                    SealType type;
                    do
                    {
                        type = poolType[rand.Next(poolType.Length)];
                    } while (modelList.ContainsKey(cellEmptyAbove) && 
                                    modelList[cellEmpty].Type == modelList[cellEmptyAbove].Type);

                    // Using Dictionary for checking duplicate same Type of seals
                    var model = new SealModel(cellEmpty.X, cellEmpty.Y, type);
                    modelList[new Vector2I(model.X, model.Y)] = model;

                    // Generate seal on Map with Y = -1
                    Seal seal = render.RespawnOneSeal(model);

                    // Add Event delegate On touch on Seal
                    render.SealTouched += _ctx.OnSealTouched;

                    // Add seal to prepare cascade animation
                    _sealsCascadeList.Add(seal);

                    // Update map data
                    _ctx.MapData[cellEmpty.X, cellEmpty.Y].Type = ObjectType.Seal;
                }
            }
        }

        /// <summary>
        /// Sort the list of null cells from the Destroy system
        /// Get the first cell in the column with the highest Y
        /// </summary>
        /// <returns>List of MapObject</returns>
        private List<MapObjectModel> SortAndGetEmptyCells()
        {      
            var emptyCells = _ctx.MapData.Cast<MapObjectModel>()     
                                        .Where(x => x.Type == ObjectType.Null)
                                        .ToList();

            List<MapObjectModel> distinct = [.. emptyCells.OrderByDescending(arr => arr.Y)
                .GroupBy(arr => arr.X)
                .Select(g => g.First())];

            return distinct;
        }

        private void ResetSealsAfterCascade()
        {
            foreach(Seal seal in _sealsCascadeList)
            {
                if(GodotObject.IsInstanceValid(seal)) seal.Reset();
            }
        }
    }

}
