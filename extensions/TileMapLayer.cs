using Godot;
using System;

public static class TileMapLayerExtensions
{
    public static Vector2 CellToWorld(this TileMapLayer layer, Vector2I index)
    {
        return new Vector2(
            layer.GlobalPosition.X + (index.X * layer.TileSet.TileSize.X) + (layer.TileSet.TileSize.X / 2),
            layer.GlobalPosition.Y + (index.Y * layer.TileSet.TileSize.Y) + (layer.TileSet.TileSize.Y / 2)
        );
    }

    public static Vector2I WorldToCell(this TileMapLayer layer, Vector2 worldPosition)
    {
        return new Vector2I(
            Mathf.FloorToInt((worldPosition.X - layer.GlobalPosition.X) / layer.TileSet.TileSize.X),
            Mathf.FloorToInt((worldPosition.Y - layer.GlobalPosition.Y) / layer.TileSet.TileSize.Y)
        );
    }
}
