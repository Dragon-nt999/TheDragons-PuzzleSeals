using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheDragonsPuzzleSeals.Features.Map
{
    public class MapAnimService()
    {
        /// <summary>
        /// Play swap animation
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static async Task PlaySwap(Seal s1, Seal s2)
        {
            List<Tween> tweens = [];

            tweens.Add(SealGlow(s1));
            tweens.Add(SealGlow(s2));
            tweens.Add(MoveTo(s1));
            tweens.Add(MoveTo(s2));

            await WaitAll(tweens);
        }

        /// <summary>
        /// Helper for await all tween
        /// </summary>
        /// <param name="tweens"></param>
        /// <returns></returns>
        public static async Task WaitAll(List<Tween> tweens)
        {
            var tasks = tweens.Where(t => t != null && t.IsValid())
                              .Select(async t => await t.ToSignal(t, Tween.SignalName.Finished));

            if(tasks.Any()) await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Move seal by swap or cascade
        /// </summary>
        /// <param name="seal"></param>
        /// <param name="delay"></param>
        /// <param name="duration"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Tween MoveTo(Seal seal, 
                                    double delay = 0, 
                                    double duration = 0.2,
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

        /// <summary>
        /// Plays cascading seals
        /// </summary>
        /// <param name="seals"></param>
        /// <returns></returns>
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
                    duration = Mathf.Clamp(duration, 0.0f, 0.6f);

                    float delay = (float)GD.RandRange(0.01, 0.1);

                    // Apply to new seal on Map
                    if(seal.Model.Action == SealAction.Refill)
                    {
                        tweens.Add(DropSealToOriginal(seal));
                    }

                    tweens.Add(MoveTo(seal, delay, duration, Animtype.Fall));
                }

                await WaitAll(tweens);
            }
        }

        /// <summary>
        /// Drop seal to mask position
        /// Use for new Seal
        /// </summary>
        /// <param name="seal"></param>
        /// <param name="duration"></param>
        /// <returns>Tween</returns>
        private static Tween DropSealToOriginal(Seal seal,
                                    double duration = 0.2f)
        {
            Tween tween = seal.CreateTween();
            
            var target = seal.Mask.Size / 2f;

            tween.TweenProperty(seal.Sprite, "position", target, duration);

            return tween;
        }

        public static Tween SealGlow(Seal seal, float glowIntensity = 2.0f, double duration = 0.1f)
        {
            Tween tween = seal.CreateTween();

            tween.TweenProperty(seal.Sprite, 
                                "self_modulate",
                                new Color(glowIntensity, glowIntensity, glowIntensity, 1f),
                                duration
                                );

            tween.TweenProperty(seal.Sprite,
                                "self_modulate",
                                Colors.White,
                                duration
                                );

            return tween;
        }
    }
}
