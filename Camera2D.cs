using Godot;

namespace Shuuut;

public partial class Camera2D : Godot.Camera2D
{
	[Export] private Node2D _follow;
	
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		GlobalPosition = _follow.GlobalPosition;
	}
}