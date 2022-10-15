using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectMouse : MonoBehaviour
{
    [SerializeField] BoxCollider col;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Cursor"))
        {
            Debug.Log("HI!");
        } 
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Cursor"))
        {
            Debug.Log("BYE");
        }
    }



}
