using Godot;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheDragonsPuzzleSeals.Core.Managers;

namespace TheDragonsPuzzleSeals.Features.Map
{
    public class DestroySystem(MapContextModel ctx)
    {
        private readonly MapContextModel _ctx = ctx;
        public async Task Execute(HashSet<Seal> sealMatches)
        {
            if (sealMatches.Count > 0)
            {
                List<Tween> tweens = [];
                foreach (var seal in sealMatches)
                {
                    if(GodotObject.IsInstanceValid(seal))
                    {
                        Tween tween = seal.CreateTween();
                        float delay = seal.Model.Y * 0.05f;
                        delay = Mathf.Clamp(delay, 0.0f, 0.05f);
                        tween.TweenInterval(delay);
                        
                        tween.Parallel().TweenProperty(seal, "scale", Vector2.One * 1.3f, 0.1f);
                        tween.Parallel().TweenProperty(seal.Sprite, "self_modulate",
                                                       new Color(2f, 2f, 2f, 1f),
                                                        0.1f);

                        tween.TweenCallback(Callable.From(
                            () =>
                            {
                                seal.QueueFree();
                                _ctx.SealViews[new Vector2I(seal.Model.X, seal.Model.Y)] = null;
                                _ctx.MapData[seal.Model.X, seal.Model.Y].Type = ObjectType.Null;
                            }
                        ));

                        tweens.Add(tween);

                        // Play Vfx Explosion
                        Tween vfxTween = VfxManager.Instance.Play(VfxType.Explosion, 
                                                                seal.VfxRemote,
                                                                Vector2.One,
                                                                seal.Model.Type.GetColor());

                        tweens.Add(vfxTween);
                    }
                }

                await MapAnimService.WaitAll(tweens);
            }
        }
    }
}

