using Godot;
using System;

namespace TheDragonsPuzzleSeals.Core.Events
{
	public partial class GameEventBus : Node
	{
		public static GameEventBus Instance { get; private set; } = null;
		public event Action<SwapExecutedEvent> SwapExecuted;
		
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			Instance = this;
		}

		public void Publish(SwapExecutedEvent @event)
		{
			SwapExecuted?.Invoke(@event);
		}
	}

}
