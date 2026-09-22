using Godot;
using System;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// A Player is an Actor that responds to user input.
/// </summary>
public partial class Player : Actor
{
	public override void _PhysicsProcess(double delta)
	{
		if (!IsActive)
		{
			return;
		}
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
		}
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
}
