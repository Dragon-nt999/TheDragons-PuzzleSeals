using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TheDragonsPuzzleSeals.Features.Map
{
    public class ResolveMatchCommand(MapContextModel ctx, Dictionary<SealType, HashSet<SealModel>> matches) : ICommand
    {
        private readonly MapContextModel _ctx = ctx;
        private Dictionary<SealType, HashSet<SealModel>> _initialMatches = matches;
        public async Task ExecuteAync()
        {
            foreach (var (key, matches) in _initialMatches)
            {
                if (matches != null)
                {
                    DestroyCommand destroyCommand = new(_ctx, matches);
                    await destroyCommand.ExecuteAync();

                    _initialMatches.Remove(key);
                }
            }
        }
    }
}

