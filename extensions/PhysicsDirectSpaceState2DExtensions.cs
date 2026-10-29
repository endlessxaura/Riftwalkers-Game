using System.Linq;
using Godot;

public static class PhysicsDirectSpaceState2DExtensions
{
    public static Godot.Collections.Array<Godot.Collections.Dictionary> IntersectCircle(
        this PhysicsDirectSpaceState2D spaceState, 
        Vector2 position, 
        float radius,
        CollisionObject2D[] exclude = default,
        uint collisionMask = 0xFFFFFFFF, 
        bool collideWithBodies = true, 
        bool collideWithAreas = true
    )
    {
        return spaceState.IntersectShape(
            new PhysicsShapeQueryParameters2D()
            {
                Shape = new CircleShape2D() { Radius = radius },
                Transform = new Transform2D(0, position),
                CollisionMask = collisionMask,
                CollideWithBodies = collideWithBodies,
                CollideWithAreas = collideWithAreas,
                Exclude = [..exclude.Select(e => e.GetRid())]
            }
        );
    }
}