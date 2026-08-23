

namespace TheDragonsPuzzleSeals.Features.Map
{
    public class MapObjectModel(int x, int y,
                            ObjectType type = ObjectType.Seal)
    {
        public int X { get; } = x;
        public int Y { get; } = y;
        public ObjectType Type { get; } = type;
    }

}