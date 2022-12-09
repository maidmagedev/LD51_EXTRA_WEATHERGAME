using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [SerializeField] Material normal;
    [SerializeField] Material glass;

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
            if (Input.GetMouseButton(0) && cell != null && !lockOut)
            {
                StartCoroutine(lockOutTimer());

                if (sGrid.blockOutMode)
                {
                    ClickToAdjustTileBlockType();
                } else if (sGrid.mouseOnHoverPathBuilding)
                {
                    path = sGrid.GetPath(player.playerCurrentCell, cell);
                    Debug.Log("Hello!");
                    sGrid.InterpretPath(path, player); // does nothing right now, to be honest.
                    if (path != null)
                    {
                        
                        player.playerCurrentCell = path.ElementAt(path.Count - 1);
                        // teleport player
                        //Debug.Log("POS?: " + player.playerCurrentCell.transform.position);
                        //player.transform.position.Set(player.playerCurrentCell.transform.position.x + 0.1f, player.transform.position.y, player.playerCurrentCell.transform.position.z + 0.1f);
                        // SMOKE AND MIRRORS
                        GameObject current = player.playerCurrentCell.gameObject.transform.GetChild(6).gameObject;
                        current.SetActive(true);
                        current.GetComponent<Renderer>().material = normal;
                        GameObject startPlayerObj = path.ElementAt(0).gameObject.transform.GetChild(6).gameObject;
                        startPlayerObj.SetActive(true);
                        startPlayerObj.GetComponent<Renderer>().material = glass;
                    }
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
        //StartCoroutine(lockOutTimer());

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
