using Godot;
using System;
using System.Diagnostics;
using System.Linq;

/// <summary>
/// This represents an arbitrary creature. It contains all of the actions the creature can take. A creature may be controlled by an Actor.
/// </summary>
public partial class Creature : StaticBody2D
{
	[Export]
	public Sprite2D BodySprite { get; set; }
	[Export]
	public float MaxHealth { get; set; } = 100f;
	[Export]
	public float MaxEnergy { get; set; } = 100f;
	public float CurrentHealth { get => _currentHealth; }
	[Export]
    public float AttackDamage { get; set; } = 10f;

	private float _currentHealth;
	private bool _outlined = false;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		_currentHealth = MaxHealth;
		GlobalPosition = new Vector2(
			(Mathf.Floor(GlobalPosition.X / Map.TileSize) * Map.TileSize) + (Map.TileSize / 2),
			(Mathf.Floor(GlobalPosition.Y / Map.TileSize) * Map.TileSize) + (Map.TileSize / 2)
		);
	}

	/// <summary>
	/// Moves a given number of tiles horizontally and vertically
	/// </summary>
	/// <param name="x">The horizontal movement. Left is negative, right is positive.</param>
	/// <param name="y">The vertical movement. Down is negative, up is positive.</param>
	/// <returns>True if the movement happened. False if something was in the way.</returns>
	/// <remarks>If this is called outside _PhysicsProcess, you will get an error.</remarks>
	public bool Move(int x, int y)
	{
		var spaceState = GetWorld2D().DirectSpaceState;
		var newPosition = GlobalPosition + (new Vector2(x, -y) * Map.TileSize);
		var collisions = spaceState.IntersectRay(PhysicsRayQueryParameters2D.Create(GlobalPosition, newPosition, uint.MaxValue, [GetRid()]));
		if (collisions.Count == 0)
		{
			Map.Instance.Unoccupy(GlobalPosition);
			GlobalPosition = newPosition;
			Map.Instance.Occupy(GlobalPosition);
			return true;
		} else
		{
			return false;
		}
	}

	/// <summary>
	/// Moves a given number of tiles horizontally and vertically
	/// </summary>
	/// <param name="movementVector">The movement vector in tiles.</param>
	/// <returns>True if the movement happened. False if something was in the way.</returns>
	/// <remarks>If this is called outside _PhysicsProcess, you will get an error.</remarks>
	public bool Move(Vector2I movementVector)
	{
		return Move(movementVector.X, -movementVector.Y);
	}


	/// <summary>
	/// Moves the creature along a path toward the specified point
	/// </summary>
	/// <param name="worldPosition">The point to move toward</param>
	/// <returns>True if any movement happened. False if there is no path toward the position.</returns>
	public bool MoveToward(Vector2 worldPosition)
	{
		Map.Instance.Unoccupy(GlobalPosition);
		var path = Map.Instance.GetPath(GlobalPosition, worldPosition);
		Map.Instance.Occupy(GlobalPosition);
		if (path.Count() > 1)
		{
			return Move(((path[1] - GlobalPosition) / Map.TileSize).RoundToInt());
		} else
		{
			return false;
		}
	}

	/// <summary>
	/// Reduces the creature's current health by the amount.
	/// </summary>
	/// <param name="amount">The amount of damage</param>
	/// <remarks>If amount if negative, no damage is dealt.</remarks>
	public void Damage(float amount)
	{
		_currentHealth = Mathf.Clamp(_currentHealth - Mathf.Max(amount, 0), 0, MaxHealth);
	}

	/// <summary>
	/// Increases the creature's current health by the amount.
	/// </summary>
	/// <param name="amount">The amount of healing.</param>
	/// <remarks>If amount if negative, no healing is provided.</remarks>
	public void Heal(float amount)
	{
		_currentHealth = Mathf.Clamp(_currentHealth + Mathf.Max(amount, 0), 0, MaxHealth);
	}

    /// <summary>
    /// Attacks another creature for this creature's AttackDamage.
    /// </summary>
    /// <param name="target">The creature to attack.</param>
    /// <returns>True if damage was dealt.</returns>
	public bool Attack(Creature target)
    {
        if (target == null || target == this || target.CurrentHealth <= 0)
        {
            return false;
        }
        target.Damage(AttackDamage);
		// TODO: animation
		// TODO: sound
        return true;
    }

	/// <summary>
	/// Applies an outline shader to the creature
	/// </summary>
	public void Outline()
	{
		((ShaderMaterial)BodySprite.Material).SetShaderParameter("outline_width", 1);
	}

	/// <summary>
	/// Removes the outline shader from the creature
	/// </summary>
	public void RemoveOutline()
	{	
		((ShaderMaterial)BodySprite.Material).SetShaderParameter("outline_width", 0);
	}
}
