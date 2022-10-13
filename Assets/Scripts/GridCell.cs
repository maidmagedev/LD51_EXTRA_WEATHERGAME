using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    [Header("== Traversability ==")]
    public BlockType blockType = BlockType.Traversable;
    public bool upOpen = true;
    public bool downOpen = true;
    public bool leftOpen = true;
    public bool rightOpen = true;

    [Header("== A* Variables ==")]
    public double costModifier = 1.0; // This is a multiplier for cost.
    public int fCost;
    public int gCost;
    public int hCost;

    public enum BlockType
    {
        Traversable,
        Untraversable,
    }



   
}
