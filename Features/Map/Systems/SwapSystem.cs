using Godot;
using System;
using System.Collections.Generic;

namespace TheDragonsPuzzleSeals.Features.Map
{
	public static class SwapSystem
	{
        public static Dictionary<String, Vector2I> CalcData(Seal seal, Vector2 distance)
		{
            int currentX = seal.Model.X;
            int currentY = seal.Model.Y;
            int targetX = currentX;
            int targetY = currentY;

            Dictionary<String, Vector2I> data = [];

            if (MathF.Abs(distance.X) > MathF.Abs(distance.Y))
            {
                targetX += distance.X > 0 ? 1 : -1;
            }
            else
            {
                targetY += distance.Y > 0 ? 1 : -1;
            }

            data["SwapFrom"] = new Vector2I(currentX, currentY);
            data["SwapTo"] = new Vector2I(targetX, targetY);

            return data;
        }

        public static void SwapData(Dictionary<Vector2I, Seal> sealViews, 
                             Vector2I from, Vector2I to)
        {

            // Update seal' model
            if (GodotObject.IsInstanceValid(sealViews[from]))
            {
                sealViews[from].Model.X = to.X;
                sealViews[from].Model.Y = to.Y;
            }

            if (GodotObject.IsInstanceValid(sealViews[to]))
            {
                sealViews[to].Model.X = from.X;
                sealViews[to].Model.Y = from.Y;
            }

            (sealViews[from], sealViews[to]) =
                    (sealViews[to], sealViews[from]);

        }
    }
}