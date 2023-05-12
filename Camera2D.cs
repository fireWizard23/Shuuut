using Godot;
using System;

public partial class Camera2D : Godot.Camera2D
{
	[Export] private Node2D follow;
	
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		GlobalPosition = follow.GlobalPosition;
	}
}
