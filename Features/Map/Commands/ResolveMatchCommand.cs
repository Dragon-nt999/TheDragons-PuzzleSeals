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
        private List<HashSet<Seal>> _initialMatches = matches;

        /// <summary>
        /// Main execution function
        /// </summary>
        /// <returns></returns>
        public async Task ExecuteAync()
        {
            while (_initialMatches.Count > 0)
            {
                await ProcessMatch();
                _initialMatches = MatchSystem.FindAndGroupMatch(_ctx);
            }
        }

        /// <summary>
        /// Calls the Destroy System to destroy matched seals
        /// Calls the Cascade System to cascading seals and spawn new seals
        /// </summary>
        /// <returns></returns>
        private async Task ProcessMatch()
        {
            List<Tween> tweensToAnimation = [];

            for (var i = _initialMatches.Count - 1; i >= 0; i--)
            {
                var match = _initialMatches[i];
                if (match.Count <= 3)
                {
                    tweensToAnimation = new DestroySystem(_ctx).Execute(match);
                } else
                {
                    tweensToAnimation = new SpawnSpecialSealsSystem(_ctx, match).Execute();
                }
                _initialMatches.RemoveAt(i);
            }

            // Play animtion destroy seal, merge seal, spawn special seal
            await MapAnimService.WaitAll(tweensToAnimation);
            
            // Play cascade seals
            await new CascadeSystem(_ctx).PlayCascadeAsync();

            // Reset All Seals with SealAction: Swap, Fall, Refill
            _ctx.ResetAllSealsByAction();   
        }
    }
}

