using Godot;
using System;
using System.Collections;
using System.Threading.Tasks;

/// <summary>
/// The ActorOrchestrator ensures each Actor gets one turn every round. The Actor determines what it does with its turn.
/// </summary>
public partial class ActorOrchestator : Node
{
	private Task _currentTurn;

    public override void _Process(double delta)
    {
        base._Process(delta);
		if (GetTree().GetNodeCountInGroup(Groups.Actors_Ready) == 0)
		{
			GetTree().CallGroup(Groups.Actors, nameof(Actor.AdvanceTurn));
		}
    }


	// Note: This must be down in _PhysicsProcess() because the actor's OnTurn might require physics operations
	public override void _PhysicsProcess(double delta)
	{
		if (_currentTurn == null || _currentTurn.IsCompleted)
		{
			var next = (Actor)GetTree().GetFirstNodeInGroup(Groups.Actors_Ready);
			_currentTurn = next?.StartTurn();
		}
	}
}
