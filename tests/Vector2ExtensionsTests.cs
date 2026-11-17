using Godot;
using Xunit;

public class Vector2ExtensionsTests
{
    [Fact]
    public void RoundToIntRoundsEachCoordinateToNearestInteger()
    {
        var result = new Vector2(1.2f, 2.8f).RoundToInt();

        Assert.Equal(new Vector2I(1, 3), result);
    }

    [Fact]
    public void RoundToIntRoundsNegativeCoordinates()
    {
        var result = new Vector2(-1.6f, -2.2f).RoundToInt();

        Assert.Equal(new Vector2I(-2, -2), result);
    }
}
