using Godot;
using System;
using System.IO;

public partial class Background : TileMap
{
	public override void _Ready()
	{
		Pathfinding.Instance.Bake(this);
	}

	public override void _Process(double delta)
	{
	}
}
