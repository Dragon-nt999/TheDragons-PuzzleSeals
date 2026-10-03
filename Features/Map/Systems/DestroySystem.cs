using Godot;
using System.Collections.Generic;
using TheDragonsPuzzleSeals.Core.Managers;

namespace TheDragonsPuzzleSeals.Features.Map
{
    public class DestroySystem(MapContextModel ctx)
    {
        private readonly MapContextModel _ctx = ctx;
        public List<Tween> Execute(HashSet<Seal> sealMatches)
        {
            List<Tween> tweens = [];
            if (sealMatches.Count > 0)
            {
                foreach (var seal in sealMatches)
                {
                    if(GodotObject.IsInstanceValid(seal))
                    {
                        // Update data
                        _ctx.SealViews[new Vector2I(seal.Model.X, seal.Model.Y)] = null;
                        _ctx.MapData[seal.Model.X, seal.Model.Y].Type = ObjectType.Null;

                        // Create Tween
                        Tween tween = seal.CreateTween();
                        float delay = seal.Model.Y * 0.05f;
                        delay = Mathf.Clamp(delay, 0.005f, 0.05f);
                        tween.TweenInterval(delay);
                        
                        tween.Parallel().TweenProperty(seal, "scale", Vector2.One * 1.3f, 0.2f);
                        tween.Parallel().TweenProperty(seal.Sprite, "self_modulate",
                                                       new Color(2f, 2f, 2f, 1f),
                                                       0.2f);

                        tween.TweenCallback(Callable.From(
                            () =>
                            {
                                seal.QueueFree();
                            }
                        ));

                        tweens.Add(tween);

                        // Play Vfx Explosion
                        VfxManager.Instance.Play(VfxType.Explosion, 
                                                  seal.VfxRemote,
                                                  Vector2.One,
                                                  seal.Model.Type.GetColor());

                    }
                }
            }

            return tweens;
        }
    }
}

