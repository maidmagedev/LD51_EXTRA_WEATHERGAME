using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://www.youtube.com/watch?v=fUiNDDcU_I4&ab_channel=RandomArtAttack

public class GridBehaviour : MonoBehaviour
{
    public int rows = 10;
    public int columns = 10;
    public int scale = 1;
    public GameObject gridPrefab;
    public Vector3 leftbottomLocation = new Vector3(0,0,0);

    private void Awake()
    {
        if (gridPrefab)
        {
            GenerateGrid();
        } else
        {
            Debug.Log("GridBehaviour.cs's is missing a valid entry to the gridPrefab field.");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 
    void GenerateGrid()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                GameObject obj = Instantiate(gridPrefab, new Vector3(leftbottomLocation.x + scale * i, leftbottomLocation.y, leftbottomLocation.z + scale*j), Quaternion.identity);
                obj.transform.SetParent(gameObject.transform);
                obj.GetComponent<GridStat>().x = i;
                obj.GetComponent<GridStat>().y = j;
            }
        }
    }
}
