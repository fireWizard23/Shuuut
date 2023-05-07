using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	public const float Speed = 300.0f;

	public override void _Process(double delta)
	{
		base._Process(delta);
		LookAt(GetGlobalMousePosition());
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		if (direction != Vector2.Zero)
		{
			velocity = direction.Normalized() * Speed;

		}
		else
		{
			velocity = velocity.MoveToward(Vector2.Zero, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
