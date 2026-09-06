using System.Diagnostics;

namespace TheDragonsPuzzleSeals.Features.Map
{
    [DebuggerDisplay("Type: {Type} | Index: {X}, {Y}")]
    public class MapObjectModel(int x, int y,
                                ObjectType ?type = null)
    {
        public int X { get; } = x;
        public int Y { get; } = y;
        public ObjectType ?Type { get; set; } = type;
    }

}