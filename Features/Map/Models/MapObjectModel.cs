

namespace TheDragonsPuzzleSeals.Features.Map
{
    public class MapObjectModel(int x, int y,
                            ObjectType ?type = null)
    {
        public int X { get; } = x;
        public int Y { get; } = y;
        public ObjectType ?Type { get; set; } = type;
    }

}