using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script detects if a mouse is hovering over this tile.
// It will enable and disable the indicator if so.

public class TileDetectMouse : MonoBehaviour
{
    [SerializeField] BoxCollider col;
    [SerializeField] GameObject indicator;
    [SerializeField] AStarGridCell cell;
    private SGrid sGrid;
    private bool lockOut = false;

    public void Awake()
    {
        sGrid = FindObjectOfType<SGrid>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Cursor"))
        {
            indicator.SetActive(true);
            //Debug.Log("test");
        } 
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Cursor"))
        {
            if (Input.GetMouseButton(0) && !lockOut && cell != null)
            {
                StartCoroutine(lockOutTimer());

                if (cell.blockType == AStarGridCell.BlockType.Traversable)
                {
                    cell.blockType = AStarGridCell.BlockType.Untraversable; 
                } else if (cell.blockType == AStarGridCell.BlockType.Untraversable)
                {
                    cell.blockType = AStarGridCell.BlockType.Traversable;
                }
                sGrid.debugAction = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Cursor"))
        {
            indicator.SetActive(false);
        }
    }

    private IEnumerator lockOutTimer()
    {
        lockOut = true;
        yield return new WaitForSeconds(0.5f);
        lockOut = false;
    }

}
