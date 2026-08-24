using Godot;

namespace TheDragonsPuzzleSeals.Features.Map
{
    public class MoveModel(Vector2 from, Vector2 to, Seal? seal = null)
    {
        public Vector2 From { get; set; } = from;
        public Vector2 To { get; set; } = to;
        public Seal? Seal { get; set; } = seal;
    }
}
