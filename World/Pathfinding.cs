using System.Collections.Generic;
using System.Linq;
using Godot;
using Color = Godot.Color;

namespace Shuuut.World;

public partial class Pathfinding : Node
{
	
	[Export]
	Vector2I _mapSize, _cellSize = Vector2I.Down * 16;
	
	[Export] private Color _enabledColor = Colors.Green; 
	[Export] private Color _disabledColor = Colors.Red;

	[Export] private Node2D _debugParent;


	public static Pathfinding Instance;

	private AStarGrid2D _aStar = new();
	private TileMap _tileMap;

	private readonly Dictionary<Vector2I, ColorRect> _cells = new();
	private Vector2I _offset;


	public override void _Ready()
	{
		base._Ready();
		Instance = this;
		_aStar.DiagonalMode = AStarGrid2D.DiagonalModeEnum.OnlyIfNoObstacles;
		Input.MouseMode =  Input.MouseModeEnum.ConfinedHidden;

	}


	public void Bake(TileMap tile)
	{
		this._tileMap = tile;
		_mapSize = tile.GetUsedRect().Size;
		_cellSize = tile.TileSet.TileSize;
		_offset = tile.GetUsedRect().Position;
		_aStar.Size = _mapSize;

		_aStar.Update();

		foreach (var usedCell in tile.GetUsedCells(0))
		{
			var rect = new ColorRect();
			_debugParent.AddChild(rect);
			rect.Size = _cellSize;
			rect.GlobalPosition = usedCell * _cellSize;
			rect.Color = _enabledColor;
			_cells.Add(usedCell, rect);
		}
	    
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
					_aStar.SetPointSolid(cell - _offset);
					this._cells[cell].Color = _disabledColor;
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
			return new();
		}
		var paths = _aStar.GetIdPath((Vector2I)fromId, (Vector2I)toId);

		return paths.Select(IdToGlobal).ToList();

	}


	public Vector2 IdToGlobal(Vector2I id)
	{
		var halfCellSize = _cellSize / 2;
		var half = new Vector2(Mathf.Sign(id.X) * halfCellSize.X, Mathf.Sign(id.Y) * halfCellSize.Y);
		id += _offset;
		return (id * _cellSize) + half;
	}

	public Vector2I ToId(Vector2 globalPos)
	{
		return _tileMap.LocalToMap(_tileMap.ToLocal(globalPos)) - _offset;
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
		for (var i = 1; i < possibleIds.Count; i++)
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