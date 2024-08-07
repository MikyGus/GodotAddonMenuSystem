using Godot;
using System.Collections.Generic;

namespace MenuControl.addons.MenuControl.Helpers;
public static class GetPaths
{
    public static IEnumerable<NodePath> GetNodePaths(this IEnumerable<Node> nodes, Node fromNode, bool shouldReturnSelf = true)
    {
        foreach (Node node in nodes)
        {
            if (shouldReturnSelf == false && node.Equals(fromNode))
            {
                continue;
            }
            yield return fromNode.GetPathTo(node);
        }
    }
}
