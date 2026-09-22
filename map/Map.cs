using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// The map allows creatures to navigate a scene.
/// </summary>
public partial class Map : Node2D
{
	private static Map _instance;
	public static Map Instance { 
		get
		{
			if (_instance == null)
			{
				_instance = new();
			}
			return _instance;
		}
	}	
	public const int TileSize = 32;

	[Export]
	public TileMapLayer[] Layers { get; set; } = [];
	public Rect2I MapRegion { get; set; }

	private AStarGrid2D _grid;

	private Map() : base() {
		_instance = this;
	}

	public override void _Ready()
	{
		base._Ready();
		ConstructGrid();
		Bake();
		GetTree().NodeAdded += Occupy;
	}

	public void Occupy(Node node)
	{
		if (node.HasCollider() && node is Node2D)
		{
			Occupy(((Node2D)node).GlobalPosition);
		}
	}

	public void Occupy(Vector2 worldPosition)
	{
		if (_grid != null)
		{
			_grid.SetPointSolid(worldPosition);
		}
	}

	public void Unoccupy(Vector2 worldPosition)
	{
		if (_grid != null)
		{
			_grid.SetPointSolid(worldPosition, false);
		}
	}

	public Vector2[] GetPath(Vector2 startPosition, Vector2 endPosition)
	{
		var path = _grid.GetIdPath(_grid.WorldToCell(startPosition), _grid.WorldToCell(endPosition), true).ToArray();
		return path.Select(x => _grid.CellToWorld(x)).ToArray();
	}

	protected void ConstructGrid()
	{
		float minX = 0;
		float maxX = 0;
		float minY = 0;
		float maxY = 0;
		foreach (var layer in Layers)
		{
			foreach (var cell in layer.GetUsedCells())
			{
				var worldPosition = layer.CellToWorld(cell);
				minX = Mathf.Min(worldPosition.X, minX);
				maxX = MathF.Max(worldPosition.X, maxX);
				minY = Mathf.Min(worldPosition.Y, minY);
				maxY = Mathf.Max(worldPosition.Y, maxY);
			}
		}
		MapRegion = new(
			Mathf.FloorToInt(minX), 
			Mathf.FloorToInt(minY), 
			Mathf.CeilToInt(maxX - minX), 
			Mathf.CeilToInt(maxY - minY)
		);
		if (_grid == null)
		{
			_grid = new()	
			{
				Region = MapRegion,
				CellSize = new Vector2(TileSize, TileSize),
				CellShape = AStarGrid2D.CellShapeEnum.Square,
				DefaultEstimateHeuristic = AStarGrid2D.Heuristic.Manhattan,
				DefaultComputeHeuristic = AStarGrid2D.Heuristic.Manhattan,
				DiagonalMode = AStarGrid2D.DiagonalModeEnum.Never
			};
		} else
		{
			_grid.Region = MapRegion;
		}
	}

	protected void Bake()
	{
		_grid.Update();
		foreach (var layer in Layers)
		{
			if (layer.CollisionEnabled)
			{
				foreach (var cell in layer.GetUsedCells())
				{
					if (layer.GetCellTileData(cell)?.GetCollisionPolygonsCount(0) > 0)
					{
						_grid.SetPointSolid(layer.CellToWorld(cell));
					}
				}
			}
		}
	}
}
