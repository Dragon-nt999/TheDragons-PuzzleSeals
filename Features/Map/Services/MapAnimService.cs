using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheDragonsPuzzleSeals.Features.Map
{
    public class MapAnimService()
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

        private static Tween MoveTo(Seal seal, Vector2 newPos, double delay = 0, double duration = 0.3)
        {
            if (!GodotObject.IsInstanceValid(seal)) return null;
            Tween tween = seal.CreateTween();

            if (tween == null) return null;

            if (delay > 0) tween.TweenInterval(delay);

            tween.TweenProperty(seal, "position", newPos, duration)
                                .SetTrans(Tween.TransitionType.Back)
                                .SetEase(Tween.EaseType.Out);

            return tween;
        }

        public static async Task PlayCascade(List<Seal> seals)
        {
            if(seals.Count > 0)
            {
                List<Tween> tweens = [];
                foreach(var seal in seals)
                {
                    if(seal == null || seal.Model == null 
                            || seal.Model.MoveTo == null)
                    {
                        return;
                    }

                    float distance = Mathf.Abs(seal.Model.MoveTo.Value.Y - seal.Position.Y);
                    float gravityFactor = 0.02f;
                    float duration = distance * gravityFactor;
                    duration = Mathf.Clamp(duration, 0.15f, 0.6f);

                    float baseDelay = distance / 1000.0f;
                    float randomOffset = (float)GD.RandRange(-0.05, 0.05);
                    float delay = Mathf.Clamp(baseDelay + randomOffset, 0.0f, 0.4f);

                    tweens.Add(MoveTo(seal, seal.Model.MoveTo.Value, delay, duration));
                }

                await WaitAll(tweens);
            }
        }
    }
}
