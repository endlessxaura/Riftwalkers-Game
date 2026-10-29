using System;
using System.Linq;
using Godot;

public class TargetSelectionState : PlayerState
{
    private Action<Creature> _onTargetSelected;
    private Creature[] _possibleTargets;

    public Creature CurrentTarget { get; private set; }
    
    public TargetSelectionState(Player player, Action<Creature> onTargetSelected) : base(player)
    {
        CurrentTarget = null;
        _onTargetSelected = onTargetSelected;
    }

    public override void ProcessTurn()
    {
        if (_possibleTargets == null)
        {
            SetPossibleTargets();
            if (_possibleTargets.Count() == 0)
            {
                player.ChangeState(new ExplorationState(player));
                return;
            }
            SetTarget(_possibleTargets.FirstOrDefault());
        }
        if (Input.IsActionJustPressed(InputBindings.Cancel))
        {
            if (CurrentTarget != null)
            {
                CurrentTarget.RemoveOutline();
            }
            player.ChangeState(new ExplorationState(player));
        }
        if (Input.IsActionJustPressed(InputBindings.Select))
        {
            SelectTarget();
        }
    }

    public void SetPossibleTargets()
    {
        var spaceState = player.GetWorld2D().DirectSpaceState;
        var collisions = spaceState.IntersectCircle(
            player.GlobalPosition, 
            player.Creature.AttackRange * Map.TileSize,
            exclude: [player.Creature]
        );
        _possibleTargets = collisions   // TODO: create a layermask for creatures
            .Select(collision => (CollisionObject2D)collision["collider"])
            .Where(collision => collision is Creature)
            .Select(collision => collision as Creature)
            .ToArray();
    }

    public void SetTarget(Creature target)
    {
        CurrentTarget = target;
        CurrentTarget.Outline(Colors.Red);
    }

    public void SelectTarget()
    {
        if (CurrentTarget != null)
        {
            _onTargetSelected(CurrentTarget);
            CurrentTarget.RemoveOutline();
        }
    }
}