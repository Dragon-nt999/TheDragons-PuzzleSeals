using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheDragonsPuzzleSeals.Features.Map
{
    public class ReFillSystem (MapContextModel ctx)
    {
        private readonly MapContextModel _ctx = ctx;
        private MapRenderService _render;

        public async Task RefillAync()
        {
            _render = new MapRenderService(_ctx);

            // Get data cell
            var sealsData = GetAndSortCellNull();

            // Generate seals
            var seals = SpawnSystem.RespawnSeals(sealsData);
            var newSeals = _render.ReSpawnSeals(seals);

            // Move new seals
            await MapAnimService.PlayCascade(newSeals);
        }

        private List<MapObjectModel> GetAndSortCellNull()
        {
            List<MapObjectModel> refillData = [];

            for(int y = _ctx.Height - 1; y >= 0; y--)
            {
                for(int x = 0; x < _ctx.Width; x++)
                {
                    var cell = _ctx.MapData[x, y];
                    if(cell != null && cell.Type == ObjectType.Null)
                    {
                        refillData.Add(cell);
                    }
                }
            }

            return refillData;
        }
    }
}