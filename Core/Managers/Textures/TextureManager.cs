using Godot;
using System;
using System.Collections.Generic;
using TheDragonsPuzzleSeals.Features.Map;

namespace TheDragonsPuzzleSeals.Core.Managers
{
	public partial class TextureManager : Node
	{
		public static TextureManager Instance { get; private set; }
		private readonly Dictionary<SealType, Texture2D> _sealTextureCache = [];
		private readonly Dictionary<FrameCellType, Texture2D> _frameCellTextureCache = [];
		private readonly Dictionary<StoneCellType, Texture2D> _stoneCellTextureCache = [];

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			if(Instance == null)
			{
				Instance = this;
				PreLoadTextures();
			} else
			{
				QueueFree();
			}
		}

		private void PreLoadTextures()
		{
			// Seal Textures
			_sealTextureCache[SealType.blue]      = GD.Load<Texture2D>("res://Assets/Textures/Seals/seal_blue.png");
			_sealTextureCache[SealType.red]       = GD.Load<Texture2D>("res://Assets/Textures/Seals/seal_red.png");
			_sealTextureCache[SealType.yellow]    = GD.Load<Texture2D>("res://Assets/Textures/Seals/seal_yellow.png");
			_sealTextureCache[SealType.green]     = GD.Load<Texture2D>("res://Assets/Textures/Seals/seal_green.png");
			_sealTextureCache[SealType.match_4_H] = GD.Load<Texture2D>("res://Assets/Textures/Seals/match_4_H.png");
			_sealTextureCache[SealType.match_4_V] = GD.Load<Texture2D>("res://Assets/Textures/Seals/match_4_V.png");
			_sealTextureCache[SealType.match_5]   = GD.Load<Texture2D>("res://Assets/Textures/Seals/match_5.png");
			_sealTextureCache[SealType.match_TL]  = GD.Load<Texture2D>("res://Assets/Textures/Seals/match_TL.png");

			// Frame Cell Textures
			_frameCellTextureCache[FrameCellType.bot_1] = GD.Load<Texture2D>("res://Assets/Textures/FrameCell/bot_1.png");
			_frameCellTextureCache[FrameCellType.bot_2] = GD.Load<Texture2D>("res://Assets/Textures/FrameCell/bot_2.png");
			_frameCellTextureCache[FrameCellType.bot_3] = GD.Load<Texture2D>("res://Assets/Textures/FrameCell/bot_3.png");
			_frameCellTextureCache[FrameCellType.bot_4] = GD.Load<Texture2D>("res://Assets/Textures/FrameCell/bot_4.png");
			_frameCellTextureCache[FrameCellType.bot_5] = GD.Load<Texture2D>("res://Assets/Textures/FrameCell/bot_5.png");
			_frameCellTextureCache[FrameCellType.bot_5] = GD.Load<Texture2D>("res://Assets/Textures/FrameCell/bot_5.png");
			_frameCellTextureCache[FrameCellType.bot_left] = GD.Load<Texture2D>("res://Assets/Textures/FrameCell/bot_left.png");
			_frameCellTextureCache[FrameCellType.bot_right] = GD.Load<Texture2D>("res://Assets/Textures/FrameCell/bot_right.png");
			_frameCellTextureCache[FrameCellType.center] = GD.Load<Texture2D>("res://Assets/Textures/FrameCell/center.png");
			_frameCellTextureCache[FrameCellType.left_1] = GD.Load<Texture2D>("res://Assets/Textures/FrameCell/left_1.png");
			_frameCellTextureCache[FrameCellType.left_2] = GD.Load<Texture2D>("res://Assets/Textures/FrameCell/left_2.png");
			_frameCellTextureCache[FrameCellType.left_3] = GD.Load<Texture2D>("res://Assets/Textures/FrameCell/left_3.png");
			_frameCellTextureCache[FrameCellType.left_4] = GD.Load<Texture2D>("res://Assets/Textures/FrameCell/left_4.png");
			_frameCellTextureCache[FrameCellType.left_5] = GD.Load<Texture2D>("res://Assets/Textures/FrameCell/left_5.png");
			_frameCellTextureCache[FrameCellType.right_1] = GD.Load<Texture2D>("res://Assets/Textures/FrameCell/right_1.png");
			_frameCellTextureCache[FrameCellType.right_2] = GD.Load<Texture2D>("res://Assets/Textures/FrameCell/right_2.png");
			_frameCellTextureCache[FrameCellType.right_3] = GD.Load<Texture2D>("res://Assets/Textures/FrameCell/right_3.png");
			_frameCellTextureCache[FrameCellType.right_4] = GD.Load<Texture2D>("res://Assets/Textures/FrameCell/right_4.png");
			_frameCellTextureCache[FrameCellType.right_5] = GD.Load<Texture2D>("res://Assets/Textures/FrameCell/right_5.png");
			_frameCellTextureCache[FrameCellType.top_1] = GD.Load<Texture2D>("res://Assets/Textures/FrameCell/top_1.png");
			_frameCellTextureCache[FrameCellType.top_2] = GD.Load<Texture2D>("res://Assets/Textures/FrameCell/top_2.png");
			_frameCellTextureCache[FrameCellType.top_3] = GD.Load<Texture2D>("res://Assets/Textures/FrameCell/top_3.png");
			_frameCellTextureCache[FrameCellType.top_4] = GD.Load<Texture2D>("res://Assets/Textures/FrameCell/top_4.png");
			_frameCellTextureCache[FrameCellType.top_5] = GD.Load<Texture2D>("res://Assets/Textures/FrameCell/top_5.png");
			_frameCellTextureCache[FrameCellType.top_left] = GD.Load<Texture2D>("res://Assets/Textures/FrameCell/top_left.png");
			_frameCellTextureCache[FrameCellType.top_right] = GD.Load<Texture2D>("res://Assets/Textures/FrameCell/top_right.png");

			// Stone Cell
			_stoneCellTextureCache[StoneCellType.cell_gray]    = GD.Load<Texture2D>("res://Assets/Textures/StoneCell/cell_gray.png");
			_stoneCellTextureCache[StoneCellType.cell_gray_1]  = GD.Load<Texture2D>("res://Assets/Textures/StoneCell/cell_gray_1.png");
			_stoneCellTextureCache[StoneCellType.cell_gray_2]  = GD.Load<Texture2D>("res://Assets/Textures/StoneCell/cell_gray_2.png");
			_stoneCellTextureCache[StoneCellType.cell_gray_3]  = GD.Load<Texture2D>("res://Assets/Textures/StoneCell/cell_gray_3.png");
			_stoneCellTextureCache[StoneCellType.cell_gray_4]  = GD.Load<Texture2D>("res://Assets/Textures/StoneCell/cell_gray_4.png");
			_stoneCellTextureCache[StoneCellType.cell_green]   = GD.Load<Texture2D>("res://Assets/Textures/StoneCell/cell_green.png");
			_stoneCellTextureCache[StoneCellType.cell_green_1] = GD.Load<Texture2D>("res://Assets/Textures/StoneCell/cell_green_1.png");
			_stoneCellTextureCache[StoneCellType.cell_green_2] = GD.Load<Texture2D>("res://Assets/Textures/StoneCell/cell_green_2.png");
			_stoneCellTextureCache[StoneCellType.cell_green_3] = GD.Load<Texture2D>("res://Assets/Textures/StoneCell/cell_green_3.png");
			_stoneCellTextureCache[StoneCellType.cell_green_4] = GD.Load<Texture2D>("res://Assets/Textures/StoneCell/cell_green_4.png");
		}

		public Texture2D GetSealTexture(SealType type)
		{
			return _sealTextureCache.GetValueOrDefault(type);
		}

		public Texture2D GetFrameCellTexture(FrameCellType type)
		{
			return _frameCellTextureCache.GetValueOrDefault(type);
		}

		public Texture2D GetStoneCellTexture(StoneCellType type)
		{
			return _stoneCellTextureCache.GetValueOrDefault(type);
		}
	}
}

