using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using TheDragonsPuzzleSeals.Core.Events;
using TheDragonsPuzzleSeals.Core.Managers;
using TheDragonsPuzzleSeals.Core.Utils;

namespace TheDragonsPuzzleSeals.Features.Map
{
    public class SpawnSpecialSealsSystem(MapContextModel ctx, HashSet<Seal> sealMatches)
    {
        private readonly MapContextModel _ctx        = ctx;
        private readonly HashSet<Seal> _sealMatches  = sealMatches;
        private SealType _typeSpecialSeal            = SealType.match_4_H;
        private Seal _specialSeal                    = null;
        private readonly List<Seal> _sealToMergeList = [];
        private bool _hasFiveSealsMatch              = false;
        private Seal _intersectSeal                  = null;

        public List<Tween> Execute()
        {
            EvaluateSpecialSeal();
            InitializeSpecialSeal();
            CollectMergeSeals();

            List<Tween> tweensList = [];
            
            tweensList.AddRange(AnimMergeSealsAndDestroy());
            tweensList.AddRange(AnimSpawnSpecialSeal());

            return tweensList;
        }

        private void EvaluateSpecialSeal()
        {
            if(_sealMatches.Count <= 4)
            {
                if(IsMatchFourHorizantal())
                {
                    _typeSpecialSeal = SealType.match_4_H;
                } else
                {
                    _typeSpecialSeal = SealType.match_4_V;
                }
            } else
            {
                FindIntersectSeal();

                if(_hasFiveSealsMatch)
                {
                    _typeSpecialSeal = SealType.match_5;
                } else
                {
                    _typeSpecialSeal = SealType.match_TL;
                }
            }
        }

        private bool IsMatchFourHorizantal()
        {
            var sealList = _sealMatches.ToList();
            for(int i = 0; i < sealList.Count - 2; i++)
            {
                Seal s1 = sealList[i];
                Seal s2 = sealList[i + 1];

                if(!GDObject.Check(s1, s2)) return false;
                
                if(s1.Model.Y != s2.Model.Y) return false;
            }

            return true;
        }

        private void FindIntersectSeal()
        {
            Seal maxSealByX = _sealMatches.MaxBy(s => s.Model.X);
            Seal maxSealByY = _sealMatches.MaxBy(s => s.Model.Y);

            Seal minSealByX = _sealMatches.MinBy(s => s.Model.X);
            Seal minSealByY = _sealMatches.MinBy(s => s.Model.Y);

            int minX = minSealByX.Model.X;
            int minY = minSealByY.Model.Y;
            int maxX = maxSealByX.Model.X;
            int maxY = maxSealByY.Model.Y;

            HashSet<Seal> sealsMatchByX = [];
            HashSet<Seal> sealsMatchByY = [];

            foreach(Seal seal in _sealMatches)
            {
                if(!GDObject.Check(seal)) return;

                int x = seal.Model.X;
                int y = seal.Model.Y;

                HashSet<Seal> matchXTemp = [];
                HashSet<Seal> matchYTemp = [];

                if(x == maxX)
                {
                    matchXTemp = FindByDiectionX(seal, minX, maxX);
                    if(matchXTemp.Count > sealsMatchByX.Count)
                    {
                        sealsMatchByX = matchXTemp;
                    }
                }

                if(y == maxY)
                {
                    matchYTemp = FindByDiectionY(seal, minY, maxY);
                    if(matchYTemp.Count > sealsMatchByY.Count)
                    {
                        sealsMatchByY = matchYTemp;
                    }
                }          
            }

            // Detect matches by 5 seals
            _hasFiveSealsMatch = sealsMatchByX.Count >= 5 || sealsMatchByY.Count >= 5;

            // Get intersect seal
            _intersectSeal = sealsMatchByX.Intersect(sealsMatchByY).FirstOrDefault();
        }

        private HashSet<Seal> FindByDiectionX( Seal seal, int minX, int maxX)
        {
            HashSet<Seal> sealsMatchByX = [];
            int x = seal.Model.X;
            int y = seal.Model.Y;

            for(int offsetX = 1; ; offsetX++)
            {
                int left  = x - offsetX;
                if(left > 0 && left >= minX)
                {
                    Seal sLeft1 = _ctx.SealViews[new Vector2I(left, y)];
                    Seal sLeft2 = _ctx.SealViews[new Vector2I(left - 1, y)];
                    if(!GDObject.Check(seal, sLeft1, sLeft2)) break;
                    if(seal.Model.Type == sLeft1.Model.Type && 
                                        sLeft1.Model.Type  == sLeft2.Model.Type)
                    {
                        sealsMatchByX.Add(seal);
                        sealsMatchByX.Add(sLeft1);
                        sealsMatchByX.Add(sLeft2);
                    }
                }
                if(left < minX)
                {
                    break;
                }
            }

            return sealsMatchByX;
        }

        private HashSet<Seal> FindByDiectionY(Seal seal, int minY, int maxY)
        {
            HashSet<Seal> sealsMatchByY = [];
            int x = seal.Model.X;
            int y = seal.Model.Y;

            for(int offsetY = 1; ; offsetY++)
            {
                int top = y - offsetY;

                if(top > 0 && top >= minY)
                {
                    Seal sTop1 = _ctx.SealViews[new Vector2I(x, top)];
                    Seal sTop2 = _ctx.SealViews[new Vector2I(x, top - 1)];
                    if(!GDObject.Check(seal, sTop1, sTop2)) break;

                    if(seal.Model.Type == sTop1.Model.Type &&
                                        sTop1.Model.Type == sTop2.Model.Type)
                    {
                        sealsMatchByY.Add(seal);
                        sealsMatchByY.Add(sTop1);
                        sealsMatchByY.Add(sTop2);
                    }
                }

                if(top < minY)
                {
                    break;
                }
            }

            return sealsMatchByY;
        }

        private void InitializeSpecialSeal()
        {
            // Initialize by mark SealAction.Swap
            foreach(var seal in _sealMatches)
            {
                if(seal.Model.Action == SealAction.Swap)
                {
                    _specialSeal = seal;
                    break;
                }
            }

            // Initialize by intersect seal or random sealin list match
            if(!GDObject.Check(_specialSeal))
            {
                if(GDObject.Check(_intersectSeal))
                {
                    _specialSeal = _intersectSeal;
                } else
                {
                    _specialSeal = _sealMatches.FirstOrDefault();
                }
            }
        }

        private void CollectMergeSeals()
        {
            if(GodotObject.IsInstanceValid(_specialSeal))
            {
                foreach(Seal seal in _sealMatches)
                {
                    if(seal != _specialSeal)
                    {
                        seal.Model.MoveTo = _ctx.ConvertPosition(_specialSeal.Model.X, _specialSeal.Model.Y);
                        _sealToMergeList.Add(seal);
                    }
                }
            }
        }

        private List<Tween> AnimMergeSealsAndDestroy()
        {
            List<Tween> tweens = [];
            foreach (var seal in _sealToMergeList)
            {
                if(GodotObject.IsInstanceValid(seal))
                {
                    // Update data
                    _ctx.SealViews[new Vector2I(seal.Model.X, seal.Model.Y)] = null;
                    _ctx.MapData[seal.Model.X, seal.Model.Y].Type = ObjectType.Null;

                    // Create Tween
                    Tween tween = seal.CreateTween();
                    float delay = seal.Model.X * 0.05f;
                    delay = Mathf.Clamp(delay, 0.005f, 0.03f);
                    tween.TweenInterval(delay);

                    tween.SetTrans(Tween.TransitionType.Quad); 
                    tween.SetEase(Tween.EaseType.In);
                    
                    // Glow
                    tween.TweenProperty(seal.Sprite,
                                "self_modulate",
                                new Color(2.0f, 2.0f, 2.0f, 1f),
                                0.2f
                                );
                    
                    // Move
                    tween.TweenProperty(seal, "position", seal.Model.MoveTo.Value, 0.2f);

                    GameEventBus.Instance.Publish(
                        new SwapExecutedEvent(
                            new Vector2I(seal.Model.X, seal.Model.Y)
                        )
                    );

                    tween.TweenCallback(Callable.From(
                        () =>
                        {
                            seal.QueueFree();
                        }
                    ));

                    tweens.Add(tween);
                }
            }

            return tweens;
        }

        private List<Tween> AnimSpawnSpecialSeal()
        {
            List<Tween> tweens = [];

            if(!GDObject.Check(_specialSeal)) return tweens;
  
            Tween tween = _specialSeal.CreateTween();

            tween.TweenInterval(0.35f);

            tween.TweenCallback(Callable.From(
                () =>
                {
                    _specialSeal.ZIndex = 9;
                    _specialSeal.Sprite.Texture = TextureManager.Instance.GetSealTexture(_typeSpecialSeal);
                }
            ));

            tween.SetTrans(Tween.TransitionType.Back); 
            tween.SetEase(Tween.EaseType.Out);

            tween.TweenProperty(_specialSeal, "scale", Vector2.One * 2f, 0.15f);

            tween.TweenProperty(_specialSeal, "scale", Vector2.One * 1.1f, 0.15f);

            tween.TweenCallback(Callable.From(
                () =>
                {
                    _specialSeal.Model.Type = _typeSpecialSeal;
                    _specialSeal.ZIndex = 0;
                }
            ));

            tweens.Add(tween);

            return tweens;
        }
    }
}