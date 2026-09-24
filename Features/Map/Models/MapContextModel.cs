using Godot;
using System;
using System.Collections.Generic;

namespace TheDragonsPuzzleSeals.Features.Map
{
    public class MapContextModel
    {
        public Node2D Node { get; set; }
        public PackedScene SealScene { get; init; }
        public PackedScene StoneCellScene { get; init; }
        public PackedScene FrameCellScene { get; init; }
        public MapObjectModel[,] MapData { get; init; }
        public float SealSize { get; init; }
        public float OffsetX { get; init; }
        public float OffsetY { get; init; }
        public int Width { get; init; }
        public int Height { get; init; }
        public Dictionary<Vector2I, Seal> SealViews = [];

        public Action<Seal, Vector2> OnSealTouched;

        /// <summary>
        /// Calculate position Seal or somethings else from SealMode[X, Y]
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Vector2 ConvertPosition(int x, int y)
        {
            float xPos = x * SealSize + OffsetX + (SealSize / 2);
            float yPos = y * SealSize + OffsetY + (SealSize / 2);

            return new Vector2(xPos, yPos);
        }

        /// <summary>
        /// Reset all seals when Resolve Match end
        /// </summary>
        public void ResetAllSeals()
        {
            foreach(var seal in SealViews)
            {
                if(seal.Value.Model.Action == SealAction.Swap)
                {
                    seal.Value.Reset();
                }
            }    
        }
    }

}
