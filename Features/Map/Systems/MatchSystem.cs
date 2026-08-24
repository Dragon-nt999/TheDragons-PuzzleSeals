using Godot;
using System.Collections.Generic;

namespace TheDragonsPuzzleSeals.Features.Map
{
    public static class MatchSystem
    {
        public static Dictionary<SealType, HashSet<SealModel>> findMatch(MapContextModel ctx)
        {
            if (ctx.SealViews == null || ctx.SealViews.Count <= 0) return null;
            Dictionary<SealType, HashSet<SealModel>> matches = [];

            Seal currentSeal = null;

            foreach (var seal in ctx.SealViews.Values)
            {
                if (seal != null && (seal.Model.Action == SealAction.Swap
                                            || seal.Model.Action == SealAction.Fall))
                {
                    currentSeal = seal;
                }

                if(currentSeal != null)
                {
                    var type = currentSeal.Model.Type;
                    HashSet<SealModel> tempMatches = [];

                    for (int x = 0; x < ctx.Width - 2; x++)
                    {
                        var s1 = ctx.SealViews[new Vector2I(x, currentSeal.Model.Y)];
                        var s2 = ctx.SealViews[new Vector2I(x + 1, currentSeal.Model.Y)];
                        var s3 = ctx.SealViews[new Vector2I(x + 2, currentSeal.Model.Y)];

                        tempMatches.UnionWith(AddMatches(s1, s2, s3, type));
                    }

                    for(int y = 0; y < ctx.Height - 2; y++)
                    {
                        var s1 = ctx.SealViews[new Vector2I(currentSeal.Model.X, y)];
                        var s2 = ctx.SealViews[new Vector2I(currentSeal.Model.X, y + 1)];
                        var s3 = ctx.SealViews[new Vector2I(currentSeal.Model.X, y + 2)];

                        tempMatches.UnionWith(AddMatches(s1, s2, s3, type));

                    }

                    if(tempMatches.Count >= 3)
                    {
                        matches[type] = tempMatches;
                    }

                    currentSeal = null;
                }
            }

            return matches;
        }

        private static HashSet<SealModel> AddMatches(Seal s1, Seal s2, Seal s3, SealType type)
        {
            HashSet<SealModel> tempMatches = [];
            if (s1 != null && s2 != null && s3 != null)
            {
                var m1 = s1.Model;
                var m2 = s2.Model;
                var m3 = s3.Model;

                if (m1 != null && m2 != null && m3 != null)
                {
                    if (IsSametype(m1, m2, type) && IsSametype(m2, m3, type))
                    {
                        tempMatches.Add(m1);
                        tempMatches.Add(m2);
                        tempMatches.Add(m3);
                    }
                }
            }

            return tempMatches;
        }

        public static bool HasMatches(Dictionary<SealType, HashSet<SealModel>> matches)
        {
            if (matches == null) return false;
            foreach(var match in matches)
            {
                if(match.Value != null && match.Value.Count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsSametype(SealModel s1, SealModel s2, SealType matchType)
        {
            return (s1.Type == s2.Type) && (s2.Type == matchType);
        }
    }
}

