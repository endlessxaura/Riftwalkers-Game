using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Godot;

/// <summary>
/// An actor that serves as a generic enemy.
/// </summary>
public partial class Enemy : Actor
{
    [Export]
    public int DetectionRadius { get; set; }

    protected override async Task OnTurn(CancellationToken cancellation)
    {
        foreach (Player player in GetTree().GetNodesInGroup(Groups.Players))
        {
            if (player.GlobalPosition.DistanceTo(GlobalPosition) <= DetectionRadius * Map.TileSize)
            {
                Creature.MoveToward(player.GlobalPosition);
            }
        }
    }
}
