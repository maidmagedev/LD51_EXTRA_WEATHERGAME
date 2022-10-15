using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script detects if a mouse is hovering over this tile.
// It will enable and disable the indicator if so.

public class TileDetectMouse : MonoBehaviour
{
    [SerializeField] BoxCollider col;
    [SerializeField] GameObject indicator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Cursor"))
        {
            indicator.SetActive(true);
            //Debug.Log("test");
        } 
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Cursor"))
        {
            indicator.SetActive(false);
        }
    }



}
