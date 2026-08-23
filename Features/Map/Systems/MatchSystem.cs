using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheDragonsPuzzleSeals.Features.Map
{
    public static class MatchSystem
    {
        public static Dictionary<SealType, HashSet<SealModel>> findMatch(MapContextModel ctx)
        {
            if (ctx.SealViews == null || ctx.SealViews.Count <= 0) return null;
            Dictionary<SealType, HashSet<SealModel>> matches = [];

            Seal currentSeal = null;

            foreach(var seal in ctx.SealViews.Values)
            {
                if (seal != null && seal.Model.Action == SealAction.Swap)
                {
                    currentSeal = seal;
                }

                if(currentSeal != null)
                {
                    var type = currentSeal.Model.Type;
                    matches[type] = new HashSet<SealModel>();

                    for (int x = 0; x < ctx.Width - 2; x++)
                    {
                        var s1 = ctx.SealViews[new Vector2I(x, currentSeal.Model.Y)].Model;
                        var s2 = ctx.SealViews[new Vector2I(x + 1, currentSeal.Model.Y)].Model;
                        var s3 = ctx.SealViews[new Vector2I(x + 2, currentSeal.Model.Y)].Model;
                        if (s1 != null && s2 != null && s3 != null)
                        {
                            if (IsSametype(s1, s2, type) && IsSametype(s2, s3, type))
                            {
                                matches[type].Add(s1);
                                matches[type].Add(s2);
                                matches[type].Add(s3);
                            }
                        }
                    }

                    for(int y = 0; y < ctx.Height - 2; y++)
                    {
                        var s1 = ctx.SealViews[new Vector2I(currentSeal.Model.X, y)].Model;
                        var s2 = ctx.SealViews[new Vector2I(currentSeal.Model.X, y + 1)].Model;
                        var s3 = ctx.SealViews[new Vector2I(currentSeal.Model.X, y + 2)].Model;
                        if (s1 != null && s2 != null && s3 != null)
                        {
                            if (IsSametype(s1, s2, type) && IsSametype(s2, s3, type))
                            {
                                matches[type].Add(s1);
                                matches[type].Add(s2);
                                matches[type].Add(s3);
                            }
                        }
                    }

                    if (matches[type].Count == 0)
                    {
                        matches.Clear();
                    }

                    currentSeal = null;
                }
            }

            return matches;
        }

        private static bool IsSametype(SealModel s1, SealModel s2, SealType matchType)
        {
            return (s1.Type == s2.Type) && (s2.Type == matchType);
        }
    }
}

