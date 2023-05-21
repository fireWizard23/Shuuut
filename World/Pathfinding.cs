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

    [Export] private Node2D debugParent;


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
		    debugParent.AddChild(rect);
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
	    Vector2I? fromId = ToId(from);
	    Vector2I? toId = ToId(to);
	    if (_aStar.IsInBoundsv((Vector2I)fromId) && _aStar.IsPointSolid((Vector2I)fromId))
	    {
		    fromId = GetClosestId(from);
	    }
	    if (_aStar.IsInBoundsv((Vector2I)toId)  && _aStar.IsPointSolid((Vector2I)toId))
	    {
		    toId = GetClosestId(to);
	    }

	    if (fromId is null || toId is null)
	    {
		    return new List<Vector2>();
	    }
	    var paths = _aStar.GetIdPath((Vector2I)fromId, (Vector2I)toId);

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
	    return tileMap.LocalToMap(tileMap.ToLocal(globalPos));
    }

    public Vector2I? GetClosestId(Vector2 worldPos)
    {
	    List<Vector2I> possibleIds = new List<Vector2I>();
	    var id = ToId(worldPos);
	    for (int y = -1; y <= 1; y++)
	    {
		    for (int x = -1; x <= 1; x++)
		    {
			    if ((x == 0 && y == 0))
			    {
				    continue;
			    }
			    var v = id + new Vector2I(x,y);
			    if (!_aStar.IsInBoundsv(v) || _aStar.IsPointSolid(v))
			    {
				    continue;
			    } 
			    
			    possibleIds.Add(v);
		    }
	    }

	    if (possibleIds.Count == 0)
	    {
		    return null;
	    }

	    var closestWorldPoint = IdToGlobal(possibleIds[0]);
	    for (int i = 1; i < possibleIds.Count; i++)
	    {
		    var current = IdToGlobal(possibleIds[i]);
		    var distanceOfCurrent = worldPos.DistanceSquaredTo(current);
		    var distanceOfClosest = worldPos.DistanceSquaredTo(closestWorldPoint);

				if (distanceOfClosest > distanceOfCurrent)
		    {
			    closestWorldPoint = current;
		    }

	    }

	    return ToId(closestWorldPoint);
    }
    
    
    
    
    
}
