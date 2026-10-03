using Godot;

namespace TheDragonsPuzzleSeals.Core.Events
{
	public sealed record SwapExecutedEvent(
		Vector2I Target
	);
}

