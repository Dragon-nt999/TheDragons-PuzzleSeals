using Godot;
using System.Collections.Generic;

namespace TheDragonsPuzzleSeals.Features.Map
{
    public static class MatchSystem
    {
        public static List<HashSet<Seal>> FindMatch(MapContextModel ctx)
        {
            if (ctx.SealViews == null || ctx.SealViews.Count == 0) return null;
            List<HashSet<Seal>> finalMatches = [];
            List<HashSet<Seal>> rawMatches   = FindAllMap(ctx);

            if(rawMatches.Count == 0) return finalMatches;

            foreach(var group in rawMatches)
            {
                var mergeGroup = new HashSet<Seal>(group);
                for(int i = finalMatches.Count - 1; i >= 0; i--)
                {
                    if(mergeGroup.Overlaps(finalMatches[i]))
                    {
                        mergeGroup.UnionWith(finalMatches[i]);
                        finalMatches.RemoveAt(i);
                    }
                }

                finalMatches.Add(mergeGroup);
            }
            
            return finalMatches;
        }

        private static List<HashSet<Seal>> FindAllMap(MapContextModel ctx)
        {
            List<HashSet<Seal>> results = [];
            HashSet<Seal> temps = [];

            //  Find by Horizonal
            for(int y = 0; y < ctx.Height; y++)
            {
                for(int x = 0; x < ctx.Width - 2; x++)
                {
                    Seal s1 = ctx.SealViews[new Vector2I(x, y)];
                    Seal s2 = ctx.SealViews[new Vector2I(x + 1, y)];
                    Seal s3 = ctx.SealViews[new Vector2I(x + 2, y)];

                    if(s1 == null || s2 == null || s3 == null) continue;

                    if(IsSametype(s1.Model, s2.Model) && IsSametype(s2.Model, s3.Model))
                    {
                        temps.Add(s1);
                        temps.Add(s2);
                        temps.Add(s3);
                    }
                }

                if(temps.Count > 2)
                {
                    results.Add(temps);
                    temps = [];
                }
            }

            //  Find by Vertical
            for(int x = 0; x < ctx.Width; x++)
            {
                for(int y = 0; y < ctx.Height - 2; y++)
                {
                    Seal s1 = ctx.SealViews[new Vector2I(x, y)];
                    Seal s2 = ctx.SealViews[new Vector2I(x, y + 1)];
                    Seal s3 = ctx.SealViews[new Vector2I(x, y + 2)];

                    if(s1 == null || s2 == null || s3 == null) continue;

                    if(IsSametype(s1.Model, s2.Model) && IsSametype(s2.Model, s3.Model))
                    {
                        temps.Add(s1);
                        temps.Add(s2);
                        temps.Add(s3);
                    }
                }

                if(temps.Count > 2)
                {
                    results.Add(temps);
                    temps = [];
                }
            }

            return results;
        } 

        public static bool HasMatches(List<HashSet<Seal>> matches)
        {
            if (matches == null) return false;
            foreach(var match in matches)
            {
                if(match != null && match.Count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsSametype(SealModel s1, SealModel s2)
        {
            return s1.Type == s2.Type;
        }
    }
}

