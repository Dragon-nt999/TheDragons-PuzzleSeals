using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TheDragonsPuzzleSeals.Features.Map
{
    public class ResolveMatchCommand(MapContextModel ctx, 
                                     List<HashSet<Seal>> matches) : ICommand
    {
        private readonly MapContextModel _ctx = ctx;
        private List<HashSet<Seal>> _initialMatches = [.. matches];

        private DestroySystem _destroySystem;
        public async Task ExecuteAync()
        {
            _destroySystem = new DestroySystem(_ctx);
            while (_initialMatches.Count > 0)
            {
                await ProcessMatch();
                _initialMatches = MatchSystem.FindMatch(_ctx);
            }    
        }

        private async Task ProcessMatch()
        {
            for (var i = _initialMatches.Count - 1; i >= 0; i--)
            {
                var match = _initialMatches[i];
                if (match != null)
                {
                    // Destroy seals
                    // and collect cell null on map
                    await _destroySystem.Execute(match);
                    _initialMatches.RemoveAt(i);
                }
            }
            
            // Play cascade
            await new CascadeSystem(_ctx, _destroySystem.CellNull).PlayCascadeAsync();

            // Respawn seals
            await new ReFillSystem(_ctx).RefillAync();
        }
    }
}

