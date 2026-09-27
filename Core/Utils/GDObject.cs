using Godot;

namespace TheDragonsPuzzleSeals.Core.Utils;
public static class GDObject
{
    public static bool Check(params GodotObject[] objects)
    {
        foreach(var o in objects)
        {
            if(!GodotObject.IsInstanceValid(o))
            {
                return false;
            }
        }

        return true;
    }
}