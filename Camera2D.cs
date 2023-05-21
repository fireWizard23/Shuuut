using Godot;
using System;

public partial class Camera2D : Godot.Camera2D
{
	[Export] private Node2D follow;
	
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		GlobalPosition = follow.GlobalPosition;
	}
}
