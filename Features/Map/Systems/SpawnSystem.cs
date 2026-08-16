using Godot;
using System;

namespace TheDragonsPuzzleSeals.Features.Map
{
    public static class SpawnSystem
    {
        public static SealModel[,] GenerateMap(int width, int height)
        {
            SealModel[,] map     = new SealModel[width, height];
            Random rand          = new Random();
            SealType[] poolType  = new SealType[]
            {
                SealType.red,
                SealType.blue,
                SealType.green,
                SealType.yellow,
            };

            for(var x = 0; x < width; x++)
            {
                for(var y = 0; y < height; y++)
                {
                    SealType type;

                    do
                    {
                        type = poolType[rand.Next(poolType.Length)];

                    } while (
                                (x >= 2 && map[x - 1, y].Type == type && map[x - 2, y].Type == type)
                              ||
                                (y >= 2 && map[x, y - 1].Type == type && map[x, y - 2].Type == type));

                    map[x, y] = new SealModel(x, y, type);
                }
            }

            return map;
        }


    }

}
