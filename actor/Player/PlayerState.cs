/// <summary>
/// This is a possible state for the player to be in.
/// </summary>
/// <param name="player">The Player.</param>
public abstract class PlayerState
{
    protected Player player { get; private set; }

    public PlayerState(Player player)
    {
        this.player = player;
    }

    /// <summary>
    /// Processes the player's turn in this state.
    /// </summary>
    public abstract void ProcessTurn();
}