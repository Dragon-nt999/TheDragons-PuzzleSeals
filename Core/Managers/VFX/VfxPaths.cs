
using System.Collections.Generic;

namespace TheDragonsPuzzleSeals.Core.Managers
{
	public static class VfxPaths
	{
		private static readonly Dictionary<VfxType, string> Paths = new()
		{
			{VfxType.Explosion, "res://Core/VFX/ExplosionVFX.tscn"}
		};

		public static string GetPath(this VfxType type)
		{
			return Paths.TryGetValue(type, out var path) ? path : string.Empty;
		}
	}
}

