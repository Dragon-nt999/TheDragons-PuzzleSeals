using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheDragonsPuzzleSeals.Features.Map
{
    public class MapAnimService
    {
        public static async Task PlaySwap(Seal s1, Seal s2, Vector2 target1, Vector2 target2)
        {
            List<Tween> tweens = [];

            tweens.Add(MoveTo(s1, target2));
            tweens.Add(MoveTo(s2, target1));

            await WaitAll(tweens);
        }

        private static async Task WaitAll(List<Tween> tweens)
        {
            var tasks = tweens.Where(t => t != null && t.IsValid())
                              .Select(async t => await t.ToSignal(t, Tween.SignalName.Finished));

            if(tasks.Any()) await Task.WhenAll(tasks);
        }

        private static Tween MoveTo(Seal seal, Vector2 newPos, double duration = 0.3)
        {
            if (!GodotObject.IsInstanceValid(seal)) return null;
            Tween tween = seal.CreateTween();

            if (tween == null) return null;

            tween.TweenProperty(seal, "position", newPos, duration)
                                .SetTrans(Tween.TransitionType.Back)
                                .SetEase(Tween.EaseType.Out);

            return tween;
        }
    }
}
