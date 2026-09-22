using Godot;
using System;
using System.Collections.Generic;

// Note: AStarGrid2D's Region is within world space. Therefore, it does not need to be adjusted by a transform.
public static class AStarGrid2DExtensions
{
    /// <summary>
    /// Converts a cell to a world position
    /// </summary>
    /// <param name="index">The cell's index</param>
    /// <returns>A world position</returns>
    public static Vector2 CellToWorld(this AStarGrid2D grid, Vector2I index)
    {
        return new Vector2(
            (index.X * grid.CellSize.X) + grid.Region.Position.X,
            (index.Y * grid.CellSize.Y) + grid.Region.Position.Y
        );
    }

    /// <summary>
    /// Converts a world position to a cell position
    /// </summary>
    /// <param name="worldPosition">The world position</param>
    /// <returns>A cell position</returns>
    public static Vector2I WorldToCell(this AStarGrid2D grid, Vector2 worldPosition)
    {
        return new Vector2I(
            Mathf.FloorToInt((worldPosition.X - grid.Region.Position.X) / grid.CellSize.X),
            Mathf.FloorToInt((worldPosition.Y - grid.Region.Position.Y) / grid.CellSize.Y)
        );
    }

    /// <summary>
    /// Disables or enables the cell at a given world position for pathing.
    /// </summary>
    /// <param name="worldPosition">The world position</param>
    /// <param name="solid">Whether disabled (true) or enabled (false)</param>
    public static void SetPointSolid(this AStarGrid2D grid, Vector2 worldPosition, bool solid = true)
    {
        grid.SetPointSolid(grid.WorldToCell(worldPosition), solid);
    }

    /// <summary>
    /// Gets all the cells within the grid. This is an iterator, so it works best in a foreach.
    /// </summary>
    /// <param name="grid"></param>
    /// <returns></returns>
    public static IEnumerable<Vector2I> GetCells(this AStarGrid2D grid)
    {
        foreach (var position in grid.GetWorldPositions())
        {
            yield return grid.WorldToCell(position);
        }
    }

    /// <summary>
    /// Gets the world position for the center of all cells within the grid. This is an iterator, so it works best in a foreach.
    /// </summary>
    /// <returns>A list of world positions</returns>
    public static IEnumerable<Vector2> GetWorldPositions(this AStarGrid2D grid)
    {
        for (float x = grid.Region.Position.X; x < grid.Region.Size.X; x += grid.CellSize.X)
        {
            for (float y = grid.Region.Position.Y; y < grid.Region.Size.Y; y += grid.CellSize.Y)
            {
                yield return new Vector2(x, y);
            }
        }
    }
}
