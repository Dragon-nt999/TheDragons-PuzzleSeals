using Godot;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TheDragonsPuzzleSeals.Features.Map
{
    public class DestroySystem(MapContextModel ctx)
    {
        private readonly MapContextModel _ctx = ctx;
        public List<MapObjectModel> CellNull { get; } = [];
        public async Task Execute(HashSet<Seal> sealMatches)
        {
            if (sealMatches.Count > 0)
            {
                foreach (var seal in sealMatches)
                {
                    if(GodotObject.IsInstanceValid(seal))
                    {
                        seal.QueueFree();
                        _ctx.SealViews[new Vector2I(seal.Model.X, seal.Model.Y)] = null;
                        _ctx.MapData[seal.Model.X, seal.Model.Y].Type = null;
                        CellNull.Add(_ctx.MapData[seal.Model.X, seal.Model.Y]);
                    }
                }
            }
        }
    }
}

