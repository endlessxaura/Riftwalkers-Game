using Godot;

/// <summary>
/// Processes the player's turn when they are exploring.
/// </summary>
/// <param name="player">The player to process.</param>
public class ExplorationState(Player player) : PlayerState(player)
{
    public override void ProcessTurn()
    {
		if (Input.IsActionJustPressed(InputBindings.Up))
		{
			TryMove(0, 1);
		}
		else if (Input.IsActionJustPressed(InputBindings.Down))
		{
			TryMove(0, -1);
		}
		else if (Input.IsActionJustPressed(InputBindings.Left))
		{
			TryMove(-1, 0);
		}
		else if (Input.IsActionJustPressed(InputBindings.Right))
		{
			TryMove(1, 0);
		} else if (Input.IsActionJustPressed(InputBindings.Attack))
        {
            player.ChangeState(new TargetSelectionState(player, target =>
            {
                player.Creature.Attack(target);
                player.EndTurn();
            }));
        }
    }
	
	/// <summary>
	/// Tries to move the player the given coordinates.
	/// </summary>
	/// <param name="x">The horizontal movement.</param>
	/// <param name="y">The vertical movement.</param>
	public void TryMove(int x, int y)
	{
		if (player.Creature.Move(x, y))
		{
			player.EndTurn();
		}
	}
}