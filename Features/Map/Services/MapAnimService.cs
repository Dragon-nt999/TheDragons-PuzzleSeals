using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheDragonsPuzzleSeals.Features.Map
{
    public class MapAnimService()
    {
        public static async Task PlaySwap(Seal s1, Seal s2)
        {
            List<Tween> tweens = [];

            tweens.Add(MoveTo(s1));
            tweens.Add(MoveTo(s2));

            await WaitAll(tweens);
        }

        private static async Task WaitAll(List<Tween> tweens)
        {
            var tasks = tweens.Where(t => t != null && t.IsValid())
                              .Select(async t => await t.ToSignal(t, Tween.SignalName.Finished));

            if(tasks.Any()) await Task.WhenAll(tasks);
        }

        private static Tween MoveTo(Seal seal, 
                                    double delay = 0, 
                                    double duration = 0.3,
                                    Animtype type = Animtype.Move)
        {
            if (!GodotObject.IsInstanceValid(seal)) return null;

            var movePos = seal.Model.MoveTo.Value;
            Tween tween = seal.CreateTween();

            if (delay > 0) tween.TweenInterval(delay);

            if(type == Animtype.Fall)
            {
                tween.SetTrans(Tween.TransitionType.Bounce); 
                tween.SetEase(Tween.EaseType.Out);
            } else
            {
                tween.SetTrans(Tween.TransitionType.Back); 
                tween.SetEase(Tween.EaseType.Out);
            }

            tween.TweenProperty(seal, "position", movePos, duration);

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
                    float gravityFactor = 0.08f;
                    float duration = distance * gravityFactor * gravityFactor;
                    duration = Mathf.Clamp(duration, 0.0f, 1.0f);

                    float delay = (float)GD.RandRange(0.01, 0.1);

                    tweens.Add(MoveTo(seal, delay, duration, Animtype.Fall));
                }

                await WaitAll(tweens);
            }
        }
    }
}
