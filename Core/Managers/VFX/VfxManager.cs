using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheDragonsPuzzleSeals.Core.Utils;

namespace TheDragonsPuzzleSeals.Core.Managers
{
	public partial class VfxManager : Node
	{
		public static VfxManager Instance { get; private set; }
		private readonly Dictionary<VfxType, PackedScene> _loadedCache = new();

		public override void _Ready()
		{
			if(Instance == null) Instance = this;
			else QueueFree();
		}

		public void PreloadVfx(params VfxType[] types)
		{
			foreach(var type in types)
			{
				string path = type.GetPath();
				if(!string.IsNullOrEmpty(path) && !_loadedCache.ContainsKey(type))
				{
					var scene = GD.Load<PackedScene>(path);
					if(scene != null) _loadedCache[type] = scene;
				}
			}
			
		}

		public Tween Play(VfxType type, 
							RemoteTransform2D remoteTransform,
							Vector2? scale = null,
							Color? color = null,
							float duration = 1f)
		{
			if(!_loadedCache.TryGetValue(type, out var scene))
			{
				scene = GD.Load<PackedScene>(type.GetPath());

				if(scene == null)
				{
					return null;
				}

				_loadedCache[type] = scene;
			}

			var vfx = scene.Instantiate<Node2D>();
			vfx.ZIndex = 10;
			GetTree().CurrentScene.AddChild(vfx);
			//vfx.GlobalPosition = position;
			remoteTransform.RemotePath = vfx.GetPath();

			// Scale Vfx
			if(scale.HasValue)
			{
				vfx.Scale = scale.Value;
			}

			// Change Color
			if(color.HasValue)
			{
                SetParticleColor(vfx, "Flash", color.Value);
                SetParticleColor(vfx, "Break", color.Value);
			}

			Tween tween = vfx.CreateTween();

			tween.TweenInterval(duration);

			tween.TweenCallback(Callable.From(
				() =>
				{
					if(GDObject.Check(remoteTransform))
					{
						remoteTransform.RemotePath = new NodePath();
					}
					vfx.QueueFree();
				}
			));

			return tween;
		}

		private static void SetParticleColor(Node parent, string nodeName, Color color)
		{
			var particle = parent.GetNodeOrNull<GpuParticles2D>(nodeName);
			if(particle != null)
			{
				particle.Modulate = color;
			}
		}

		public void ClearCache()
		{
			_loadedCache.Clear();
		}
	}

}
