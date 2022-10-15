using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

public class SGrid : MonoBehaviour
{
    [Header("Grid")]
    public bool wasGenerated = false;
    public bool debugAction = false;
    public GameObject[,] gridArray;
    public char[,] gridData;

    [Header("Grid Settings")]
    [SerializeField] GameObject pfGridCell; // grid prefab
    [SerializeField] int rows = 10;
    [SerializeField] int columns = 10;
    [SerializeField] Vector3 startingLocation = new Vector3(0, 0, 0);
    [SerializeField] string gridFileName = "Log.text";




    private void Awake()
    {
        if (gridArray == null)
        {
            Debug.Log("declaring new Grid Array");
            gridArray = new GameObject[rows, columns];
        }
        if (pfGridCell != null && !wasGenerated)
        {
            CreateGrid();
            GetGridDataFromFile();
            
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
                    AStarGridCell.BlockType type = gridCell.GetComponent<AStarGridCell>().blockType;
                    if (type == AStarGridCell.BlockType.Untraversable)
                    {
                        //Debug.Log("Untraversable at [" + rowIndex + "][" + colIndex + "]");
                        gridCell.transform.GetChild(4).gameObject.SetActive(true);
                    } else if (type == AStarGridCell.BlockType.Traversable)
                    {
                        gridCell.transform.GetChild(4).gameObject.SetActive(false);
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
                GameObject gridCell = Instantiate(pfGridCell, new Vector3(startingLocation.x + colIndex, 1, startingLocation.z + rowIndex), Quaternion.identity);
                gridCell.transform.SetParent(gameObject.transform); // Sets this created cell object as a child of this scriptholder
                gridArray[rowIndex, colIndex] = gridCell;
            }
        }
        wasGenerated = true;
    }

    void GetGridDataFromFile()
    {
        string filePath = Application.streamingAssetsPath + "/Levels/" + gridFileName;
        Debug.Log(File.ReadAllText(filePath));
        List<string> fileLines = File.ReadAllLines(filePath).ToList();
        
        //string data = File.ReadAllText(filePath);

        
        for (int rowIndex = 0; rowIndex < rows; rowIndex++)
        {
            string data = fileLines[rowIndex];
            for (int colIndex = 0; colIndex < columns; colIndex++)
            {
                //gridData[rowIndex, colIndex] = data[colIndex];
                //Debug.Log(data[colIndex]);

                if (data[colIndex] == 'x')
                {
                    gridArray[rowIndex, colIndex].GetComponent<AStarGridCell>().blockType = AStarGridCell.BlockType.Untraversable;
                }
            }
            
        }
        //debugAction = true;
        
    }
}
