using Godot;

public static class Vector2Extensions
{
    public static Vector2I RoundToInt(this Vector2 vector)
    {
        return new Vector2I(Mathf.RoundToInt(vector.X), Mathf.RoundToInt(vector.Y));
    }
}