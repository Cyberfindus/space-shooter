using Godot;
namespace SpaceShooter.Scripts;

public sealed partial class Player : Sprite2D
{
	[Export]
	private float _startX = 570F;

	[Export]
	private float _startY = 520F;

	[Export]
	private float _speed = 120F;

	[Export]
	private float _maxY = 580F;

	[Export]
	private float _minY = 400F;

	private static readonly StringName moveUpAction = "move_up";
	private static readonly StringName moveDownAction = "move_down";
	private static readonly StringName moveLeftAction = "move_left";
	private static readonly StringName moveRightAction = "move_right";

	private PlayerBoundaries _boundaries;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Position = new Vector2(_startX, _startY);
		_boundaries = DetermineBoundaries();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Move(delta);
	}

	private void Move(double delta)
	{
		var movementDirection = Vector2.Zero;

		if (Input.IsActionPressed(moveUpAction) && Position.Y > _boundaries.MinY)
		{
			movementDirection += Vector2.Up;
		}
		if (Input.IsActionPressed(moveDownAction) && Position.Y < _boundaries.MaxY)
		{
			movementDirection += Vector2.Down;
		}
		if (Input.IsActionPressed(moveLeftAction))
		{
			movementDirection += Vector2.Left;
		}
		if (Input.IsActionPressed(moveRightAction))
		{
			movementDirection += Vector2.Right;
		}

		if (movementDirection == Vector2.Zero)
		{
			return;
		}
		Translate(movementDirection * (_speed * (float)delta));

		if (Position.X < _boundaries.MinX)
		{
			Position = new Vector2(_boundaries.MaxX, Position.Y);
		}
		else if (Position.X > _boundaries.MaxX)
		{
			Position = new Vector2(_boundaries.MinX, Position.Y);
		}
	}
	private PlayerBoundaries DetermineBoundaries()
	{
		// still does not handle window resizing
		var windowSize = GetViewportRect().Size;
		var textureSize = Texture.GetSize();
		var scaledSize = textureSize * Scale;
		var halfSize = scaledSize / 2F;
		return new(halfSize.X,
		windowSize.X - halfSize.X,
		_minY,
		_maxY);
	}
	private readonly record struct PlayerBoundaries(float MinX, float MaxX, float MinY,
	float MaxY);
}
