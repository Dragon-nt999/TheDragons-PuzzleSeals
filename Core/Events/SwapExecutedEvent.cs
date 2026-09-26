using Godot;
using TheDragonsPuzzleSeals.Features.Map;

namespace TheDragonsPuzzleSeals.Core.Events
{
	public sealed record SwapExecutedEvent(
		Vector2I Target1,
		Vector2I Target2
	);
}

