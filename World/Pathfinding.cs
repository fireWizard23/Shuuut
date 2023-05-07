using Godot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Color = Godot.Color;

public partial class Pathfinding : Node
{
	
	[Export]
	Vector2I MapSize, CellSize = Vector2I.Down * 16;
	
	[Export] private Color enabledColor = Colors.Green; 
    [Export] private Color disabledColor = Colors.Red;


    private AStarGrid2D _aStar = new();

    public static Pathfinding Instance;
    private TileMap tileMap;

    private Dictionary<Vector2I, ColorRect> cells = new();

    public override void _Ready()
    {
	    base._Ready();
	    Instance = this;
	    _aStar.DiagonalMode = AStarGrid2D.DiagonalModeEnum.OnlyIfNoObstacles;


    }


    public void Bake(TileMap tile)
    {
	    this.tileMap = tile;
	    MapSize = tile.GetUsedRect().Size;
	    CellSize = tile.TileSet.TileSize;
	    _aStar.Size = MapSize;
	    foreach (var usedCell in tile.GetUsedCells(0))
	    {
		    var rect = new ColorRect();
		    AddChild(rect);
		    rect.Size = CellSize;
		    rect.GlobalPosition = usedCell * CellSize;
		    rect.Color = enabledColor;
		    cells.Add(usedCell, rect);
	    }
	    
	    _aStar.Update();
    }

    public override void _PhysicsProcess(double delta)
    {
	    base._PhysicsProcess(delta);
	    var nodes = GetTree().GetNodesInGroup("navigation_solids");
	    foreach (var node in nodes)
	    {
		    if (node is TileMap tilemap)
		    {
			    var cells = tilemap.GetUsedCells(0);
			    foreach (var cell in cells)
			    {
				    _aStar.SetPointSolid(cell);
				    this.cells[cell].Color = disabledColor;
			    }
		    }
	    }
    }

    public List<Vector2> GetPath(Vector2 from, Vector2 to)
    {
	    var fromId = ToId(from);
	    var toId = ToId(to);
	    var paths = _aStar.GetIdPath(fromId, toId);

	    return paths.Select(IdToGlobal).ToList();

    }


    public Vector2 IdToGlobal(Vector2I id)
    {
	    var halfCellSize = CellSize / 2;
	    var half = new Vector2(Mathf.Sign(id.X) * halfCellSize.X, Mathf.Sign(id.Y) * halfCellSize.Y);
		    
	    return (id * CellSize) + half;
    }

    public Vector2I ToId(Vector2 globalPos)
    {
	    // return (Vector2I)(globalPos / CellSize);
	    return tileMap.LocalToMap(tileMap.ToLocal(globalPos));
    }
    
    
    
    
    
}
