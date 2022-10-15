using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SGrid : MonoBehaviour
{
    [Header("Grid")]
    public bool wasGenerated = false;
    public bool debugAction = false;
    public GameObject[,] gridArray;

    [Header("Grid Settings")]
    [SerializeField] GameObject pfGridCell; // grid prefab
    [SerializeField] int rows = 10;
    [SerializeField] int columns = 10;
    [SerializeField] Vector3 startingLocation = new Vector3(0, 0, 0);

    


    private void Awake()
    {
        if (gridArray == null)
        {
            Debug.Log("declaring new Grid Array");
            gridArray = new GameObject[columns, rows];
        }
        if (pfGridCell != null && !wasGenerated)
        {
            CreateGrid();
            EditorUtility.SetDirty(gameObject);
            Debug.Log(EditorUtility.IsDirty(gameObject));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (debugAction)
        {
            for (int rowIndex = 0; rowIndex < rows; rowIndex++)
            {
                for (int colIndex = 0; colIndex < columns; colIndex++)
                {
                    GameObject gridCell = gridArray[rowIndex, colIndex];
                    if (gridCell.GetComponent<AStarGridCell>().blockType == AStarGridCell.BlockType.Untraversable)
                    {
                        Debug.Log("Untraversable at [" + rowIndex + "][" + colIndex + "]");
                        gridCell.transform.GetChild(1).gameObject.SetActive(true);
                    }
                }
            }
            Debug.Log("Debug action complete.");
            debugAction = false;
        }
    }

    // Creates the grid
    void CreateGrid()
    {
        for (int rowIndex = 0; rowIndex < rows; rowIndex++)
        {
            for (int colIndex = 0; colIndex < columns; colIndex++)
            {
                GameObject gridCell = Instantiate(pfGridCell, new Vector3(startingLocation.x + rowIndex, startingLocation.y, startingLocation.z + colIndex), Quaternion.identity);
                gridCell.transform.SetParent(gameObject.transform); // Sets this created cell object as a child of this scriptholder
                gridArray[rowIndex, colIndex] = gridCell;
            }
        }
        wasGenerated = true;
    }
}
