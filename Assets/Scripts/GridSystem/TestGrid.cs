using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGrid : MonoBehaviour
{
    private Grid grid;

    // Start is called before the first frame update
    void Start()
    {
        grid = new Grid(4, 2, 10f);
        //mousePos = new MousePosition3D();
    }

    private void Update()
    {
        
    }

}
