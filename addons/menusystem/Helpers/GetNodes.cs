using Godot;
using System.Collections.Generic;

namespace MenuControl.addons.MenuControl.Helpers;
public static class GetNodes
{
    public static IEnumerable<Node> GetAllChildren(Node node)
    {
        Godot.Collections.Array<Node> childrens = node.GetChildren();
        foreach (Node child in childrens)
        {
            yield return child;
            foreach (Node grandChild in GetAllChildren(child))
            {
                yield return grandChild;
            }
        }
    }

    /// <summary>
    /// Gets all nodes of the specified class. NOTE!!! The specified class need to have the[Tool] attribute!!
    /// </summary>
    /// <typeparam name="T">The class we are searching for</typeparam>
    /// <param name="node">The node we start our search.</param>
    /// <returns>An IEnumerable collection of all nodes found</returns>
    public static IEnumerable<T> GetAllChildren<T>(Node node) where T : Node
    {
        Godot.Collections.Array<Node> childrens = node.GetChildren();
        foreach (Node child in childrens)
        {
            if (child is T c)
            {
                yield return c;
            }
            foreach (T grandChild in GetAllChildren<T>(child))
            {
                if (grandChild is T gc)
                {
                    yield return gc;
                }
            }
        }
    }

    public static T GetParent<T>(Node node, int maxLevelToSearch = 50) where T : Node
    {
        Node currentNode = node;
        for (int i = 0; i < maxLevelToSearch; i++)
        {
            Node parentNode = currentNode.GetParent();
            if (parentNode is null)
            {
                GD.PushError($"{node.Name} failed to find parent as type {typeof(T)} at iteration {i} with a search level of {maxLevelToSearch}");
                return null;
            }
            else if (parentNode is T targetNode)
            {
                return targetNode;
            }
            else
            {
                currentNode = parentNode;
            }
        }
        GD.PushError($"{node.Name} failed to find parent as type {typeof(T)} with a search level of {maxLevelToSearch}");
        return null;
    }
}
