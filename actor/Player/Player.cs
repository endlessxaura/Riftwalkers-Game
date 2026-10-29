using Godot;
using System;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// A Player is an Actor that responds to user input.
/// This works as a state machine.
/// </summary>
public partial class Player : Actor
{
	protected PlayerState CurrentState { get; set; }

	public override void _PhysicsProcess(double delta)
	{
		if (!IsActive)
		{
			return;
		}
		if (CurrentState == null)
		{
			CurrentState = new ExplorationState(this);
		}
		CurrentState.ProcessTurn();
	}
	
	protected override async Task OnTurn(CancellationToken token)
	{
		// Creature.Outline();
		while (IsActive && !token.IsCancellationRequested)
		{
			await Task.Delay(100);
		}
		// Creature.RemoveOutline();
		return;
	}

	public override void EndTurn()
	{
		base.EndTurn();
		CurrentState = new ExplorationState(this);
	}

	public void ChangeState(PlayerState newState)
	{
		if (CurrentState.GetType() == newState.GetType())
		{
			return;
		}
		CurrentState = newState;
	}
}
