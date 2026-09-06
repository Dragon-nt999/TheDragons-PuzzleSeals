using Godot;
using System;
using System.Diagnostics;
namespace TheDragonsPuzzleSeals.Features.Map
{
    [DebuggerDisplay("Type: {Model.Type} | Index: {Model.X}, {Model.Y}")]
    public partial class Seal : Area2D
    {
        [Signal]
        public delegate void SealTouchedEventHandler(Seal seal, Vector2 mousePosition);
        public Sprite2D Sprite {get; set;}
        public Control Mask {get; set;}
        public SealModel Model;
        public override void _Ready()
        {
            Mask   = GetNode<Control>("Mask");
            Sprite = GetNode<Sprite2D>("Mask/Object");
            InputEvent += OnTouchedEvent;
        }

        /// <summary>
        /// Initial Seal from SealModel[X, Y, Type]
        /// and Seal size, which get from Map
        /// </summary>
        /// <param name="model"></param>
        /// <param name="size"></param>
        public void Initialize(SealModel model, float size)
        {
            if (model == null) return;
            Model = model;

            Vector2 textureSize = Sprite.Texture.GetSize();
            Vector2 scale       = new(size / textureSize.X, size / textureSize.Y);
            Mask.Scale          = scale;

            string texturePath = $"res://Assets/Textures/Seals/seal_{model.Type}.png";

            try
            {
                Sprite.Texture = GD.Load<Texture2D>(texturePath);
            } catch(Exception e)
            {
                GD.Print("Error when loading texture " + e.Message);
            }
        }

        /// <summary>
        /// Detect Seal when user click or touch on seal
        /// get Seal and current position of mouse 
        /// </summary>
        /// <param name="viewport"></param>
        /// <param name="event"></param>
        /// <param name="shapeIdx"></param>
        private void OnTouchedEvent(Node viewport, InputEvent @event, long shapeIdx)
        {
            if (@event is InputEventMouseButton mouseButton &&
                        mouseButton.ButtonIndex == MouseButton.Left)
            {
                if (mouseButton.Pressed)
                {
                    EmitSignal(SignalName.SealTouched, this, mouseButton.Position);
                }
            }
        }

        public void Reset()
        {
            Model.Action = null;
            Model.MoveTo = null;
        }

        public override void _ExitTree()
        {
            InputEvent -= OnTouchedEvent;
        }
    }
}

