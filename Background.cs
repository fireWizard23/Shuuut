using Godot;
using System;
using System.IO;

public partial class Background : TileMap
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Pathfinding.Instance.Bake(this);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
