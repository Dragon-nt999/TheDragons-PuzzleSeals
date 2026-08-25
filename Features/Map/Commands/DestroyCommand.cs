using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TheDragonsPuzzleSeals.Features.Map
{
    public class DestroyCommand(MapContextModel ctx, HashSet<SealModel> matches) : ICommand
    {
        private readonly MapContextModel _ctx = ctx;
        private readonly HashSet<SealModel> _matches = matches;
        public async Task ExecuteAync()
        {
            if(_matches != null && _matches.Count > 0)
            {
                foreach(var model in _matches)
                {
                    if(model != null)
                    {
                        var pos = new Vector2I(model.X, model.Y);
                        _ctx.SealViews[pos].QueueFree();
                        _ctx.SealViews[pos] = null;
                        _ctx.MapData[pos.X, pos.Y].Type = null;
                    }
                }
            }
        }
    }
}

