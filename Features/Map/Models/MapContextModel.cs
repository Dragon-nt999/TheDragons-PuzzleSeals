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
        //public Func<int, int, Vector2> ConvertPosition { get; init; }
        public Dictionary<Vector2I, Seal> SealViews = [];

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

        public void SwapData(Vector2I current, Vector2I target)
        {

            // Update seal' model
            if (GodotObject.IsInstanceValid(SealViews[current]))
            {
                SealViews[current].Model.X = target.X;
                SealViews[current].Model.Y = target.Y;
            }

            if (GodotObject.IsInstanceValid(SealViews[target]))
            {
                SealViews[target].Model.X = current.X;
                SealViews[target].Model.Y = current.Y;
            }


            (SealViews[current], SealViews[target]) =
                    (SealViews[target], SealViews[current]);

        }

        public void ResetDataAllSeals()
        {
            foreach(var seal in SealViews)
            {
                if(seal.Value != null && seal.Value.Model.Action != null)
                {
                    seal.Value.Reset();
                }
            }
        }
    }

}
