using Godot;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// This is the base of all actors. An actor controls a creature during its turn.
/// </summary>
public abstract partial class Actor : Node2D
{
	private CancellationTokenSource _activeTurnCancellation = new CancellationTokenSource();
	private Task _activeTurn;
	private bool _isActive = false;

	public bool IsActive { get => _isActive; }
	public new Vector2 GlobalPosition { get => Creature.GlobalPosition; set => Creature.GlobalPosition = value; }

	[Export]
	public Creature Creature { get; set; }

    #region Godot Methods
    public override void _EnterTree()
    {
        base._EnterTree();
		AddToGroup(Groups.Actors);
		AddToGroup(Groups.Actors_Ready);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
		RemoveFromGroup(Groups.Actors);
		RemoveFromGroup(Groups.Actors_Ready);
    }
	#endregion

	#region Turn System
	/// <summary>
	/// Advances toward the actor's next turn.
	/// </summary>
	public void AdvanceTurn()
	{
		if (!_isActive && !IsInGroup(Groups.Actors_Ready))
		{
			AddToGroup(Groups.Actors_Ready);
		}
	}

	/// <summary>
	/// This is called when it is this actor's turn.
	/// </summary>
	/// <returns>A task that ends when the turn is over.</return>
	public async Task StartTurn()
	{
		_isActive = true;
		_activeTurn = OnTurn(_activeTurnCancellation.Token);
		await _activeTurn;
		EndTurn();
	}

	/// <summary>
	/// This performs the actions during the actor's turn. It must be implemented by the actor.
	/// </summary>
	/// <param name="cancellation">Indicates whether a cancellation is requested.</param>
	/// <returns>A task that ends when the turn is over.</return>
	protected abstract Task OnTurn(CancellationToken cancellation);

	/// <summary>
	/// This ends this actors turn. It can be used to cancel an ongoing turn.
	/// </summary>
	protected void EndTurn()
	{
		if (_activeTurn != null && !_activeTurn.IsCompleted)
		{
			_activeTurnCancellation.Cancel();
			_activeTurnCancellation = new CancellationTokenSource();
		}
		RemoveFromGroup(Groups.Actors_Ready);
		_isActive = false;
	}
	#endregion
	
	#region Controller functions
	protected void TryMove(int x, int y)
	{
		if (Creature.Move(x, y))
		{
			EndTurn();
		}
	}
	#endregion
}
