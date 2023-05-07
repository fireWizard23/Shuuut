using Godot;
using System;

public partial class Pathfinding : Node
{
	[Export] private Vector2I MapSize, CellSize = new Vector2I(1, 1);
	
	
	public AStarGrid2D Astar { get; private set; }= new AStarGrid2D();
	
	public static Pathfinding Instance { get; private set; }
	
	
	public override void _Ready()
	{
		Instance = this;
		Astar.Size = MapSize;
		Astar.CellSize = CellSize;
		Astar.Update();
		
	}

	
	

	public override void _PhysicsProcess(double delta)
	{
		var h = GetTree().GetNodesInGroup("navigation_solids");
		foreach (var node in h)
		{
			if (node is TileMap tileMap)
			{
				var usedCells = tileMap.GetUsedCells(0);
				foreach (var usedCell in usedCells)
				{
					Astar.SetPointSolid(usedCell);
				}
			}
		}
	}
	
	

}
