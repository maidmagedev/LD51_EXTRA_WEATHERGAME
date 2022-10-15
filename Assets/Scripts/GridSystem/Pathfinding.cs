using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    List<GameObject> OPEN = new List<GameObject>();
    List<GameObject> CLOSED = new List<GameObject>();
    GameObject[,] gridArray;
    GameObject start;
    GameObject end;
    
    // Start is called before the first frame update
    void Start()
    {
        gridArray = GetComponentInParent<SGrid>().gridArray;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
