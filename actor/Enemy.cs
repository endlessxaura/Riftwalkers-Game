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
        // TODO: if multiple players, prioritize
        foreach (Player player in GetTree().GetNodesInGroup(Groups.Players))
        {
            float distance = player.GlobalPosition.DistanceTo(GlobalPosition);
            if (distance <= DetectionRadius * Map.TileSize)
            {
                if (distance <= Map.TileSize) // TODO: add range
                {
                    Creature.Attack(player.Creature);
                }
                else
                {
                    Creature.MoveToward(player.GlobalPosition);
                }
                return;
            }
        }
    }
}
