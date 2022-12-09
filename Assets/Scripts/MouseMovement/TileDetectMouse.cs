using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script detects if a mouse is hovering over this tile.
// It will enable and disable the indicator if so.
// Does a lot of other stuff than just detecting mouse hover.

public class TileDetectMouse : MonoBehaviour
{
    [SerializeField] BoxCollider col;
    [SerializeField] GameObject indicator;
    [SerializeField] AStarGridCell cell;
    private SGrid sGrid;
    private bool lockOut = false;
    private PlayerMovement player;
    private List<AStarGridCell> path;

    public void Awake()
    {
        sGrid = FindObjectOfType<SGrid>();
        sGrid.showUntraversable = true;
        player = FindObjectOfType<PlayerMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Cursor"))
        {
            indicator.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Cursor"))
        {
            if (Input.GetMouseButton(0) && cell != null)
            {
                if (sGrid.blockOutMode && !lockOut)
                {
                    ClickToAdjustTileBlockType();
                } else if (sGrid.mouseOnHoverPathBuilding)
                {
                    path = sGrid.GetPath(player.playerCurrentCell, cell);

                }

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Cursor"))
        {
            indicator.SetActive(false);
            /*
            if (path != null)
            {
                sGrid.clearPath(path);
                path = null;
            }*/
        }
    }

    private IEnumerator lockOutTimer()
    {
        lockOut = true;
        yield return new WaitForSeconds(0.5f);
        lockOut = false;
    }

    private void ClickToAdjustTileBlockType()
    {
        StartCoroutine(lockOutTimer());

        if (cell.blockType == AStarGridCell.BlockType.Traversable)
        {
            cell.blockType = AStarGridCell.BlockType.Untraversable;
        }
        else if (cell.blockType == AStarGridCell.BlockType.Untraversable)
        {
            cell.blockType = AStarGridCell.BlockType.Traversable;
        }
        sGrid.showUntraversable = true;
    }
}
