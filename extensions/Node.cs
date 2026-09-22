using System.Collections.Generic;
using Godot;

public static class NodeExtensions
{
    public static bool HasCollider(this Node node)
    {
        Queue<Node> nodesToCheck = new();
        nodesToCheck.Enqueue(node);
        while (nodesToCheck.Count > 0)
        {
            var currentNode = nodesToCheck.Dequeue();
            if (currentNode is CollisionObject2D || currentNode is CollisionObject3D)
            {
                return true;
            }
            foreach (var child in currentNode.GetChildren())
            {
                nodesToCheck.Enqueue(child);
            }
        }
        return false;
    }
}