using Godot;
using System;
using System.Collections.Generic;

namespace TheDragonsPuzzleSeals.Features.Map
{
    public static class SpawnSystem
    {
        public static Dictionary<Vector2I, SealModel> SpawnSeals(MapObjectModel[,] map)
        {
            Dictionary<Vector2I, SealModel> seals = [];
            Random rand          = new Random();
            SealType[] poolType  = new SealType[]
            {
                SealType.red,
                SealType.blue,
                SealType.green,
                SealType.yellow,
            };

            foreach(var obj in map)
            {
                SealType type;

                do
                {
                    type = poolType[rand.Next(poolType.Length)];
                } while (
                            (obj.X >= 2 && seals[new Vector2I(obj.X - 1, obj.Y)].Type 
                                                   == type && seals[new Vector2I(obj.X - 2, obj.Y)].Type == type)
                          ||
                            (obj.Y >= 2 && seals[new Vector2I(obj.X, obj.Y - 1)].Type 
                                                   == type && seals[new Vector2I(obj.X, obj.Y - 2)].Type == type));

                seals[new Vector2I(obj.X, obj.Y)] = new SealModel(obj.X, obj.Y, type);
                obj.Type = ObjectType.Seal;
            }

            return seals;
        }

        

    }

}
