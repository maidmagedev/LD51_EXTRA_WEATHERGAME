using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarGridCell : MonoBehaviour
{
    [Header("== Traversability ==")]
    public BlockType blockType = BlockType.Traversable;
    public bool upOpen = true;
    public bool downOpen = true;
    public bool leftOpen = true;
    public bool rightOpen = true;

    [Header("== A* Variables ==")]
    public double costModifier = 1.0; // This is a multiplier for cost.
    public int fCost = int.MaxValue;
    public int gCost = int.MaxValue;
    public int hCost = 0;
    public AStarGridCell parent;

    [Header("== Pathfinding ==")]
    // These positions are updated by SGrid.cs
    public int x;
    public int y;
    public int weight; // used for the preliminary Dijkstra's variant. This file is erroneously used for Dijkstra's as of now.
    public bool visited;
    public List<AStarGridCell> neighbors = new List<AStarGridCell>(4);

    public enum BlockType
    {
        Traversable,
        Untraversable,
    }

    public int CalculateFCost()
    {
        fCost = gCost + hCost;
        return fCost;
    }

    public void ClearCosts()
    {
        fCost = int.MaxValue;
        gCost = int.MaxValue;
        hCost = 0;
    }

    
    public double getDistToCell(AStarGridCell b)
    {
        // I suspect that this will have trouble with obstacles.
        int width = Mathf.Abs(x - b.x);
        int height = Mathf.Abs(y - b.y);
        //return Mathf.Sqrt((width * width) + (height * height)); // pythag dist
        return width + height; // manhattan? 
    }

    public int compareTo(AStarGridCell b)
    {
        return fCost - b.fCost;
    }
}
