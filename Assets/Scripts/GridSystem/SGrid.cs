using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

// This script creates a 2D array of gameobjects. These gameobjects are Grid cells that should contain the script "AStarGridCell.cs".
// These gameobjects are instantiated in the world.

public class SGrid : MonoBehaviour
{
    [Header("Grid")]
    public bool wasGenerated = false;
    public bool showUntraversable = false;
    public GameObject[,] gridArray;
    public char[,] gridData;

    [Header("Grid Settings")]
    [SerializeField] GameObject pfGridCell; // grid prefab - Use the one named "DebugTile".
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
        if (showUntraversable)
        {
            ShowUntraversableTiles();
            Debug.Log("Debug action complete.");
            showUntraversable = false;
        }
    }

    // Enables all valid "untraversable" indicators.
    public void ShowUntraversableTiles()
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
                }
                else if (type == AStarGridCell.BlockType.Traversable)
                {
                    gridCell.transform.GetChild(4).gameObject.SetActive(false);
                }
            }
        }
    }

    // Creates the grid and instantiates the pfGridCell objects.
    void CreateGrid()
    {
        for (int rowIndex = 0; rowIndex < rows; rowIndex++)
        {
            for (int colIndex = 0; colIndex < columns; colIndex++)
            {
                GameObject gridCell = Instantiate(pfGridCell, new Vector3(startingLocation.x + colIndex, 1, startingLocation.z + rowIndex), Quaternion.identity);
                gridCell.transform.SetParent(gameObject.transform); // Sets this created cell object as a child of this scriptholder
                gridArray[rowIndex, colIndex] = gridCell; // Grid array stores the representations of each cell.
            }
        }
        wasGenerated = true;
    }

    // Should only be called once, but won't do anything catastrophic otherwise.
    // Given the fileName provided by the SerializeField, update all the grid cells in the gridArray to the corresponding settings.
    // The contents must manually be checked to be valid. Unexpected behaviors may occur if the text file does not exist, or if the contents of the file are invalid.
    // Invalid meaning an inaccurate representation of rows/cols.
    // TO ENSURE PROPER TEXT FILES, refrain from editting them manually and instead rely on the "SaveToFile" prefab and click the "doThing" bool box.
    void GetGridDataFromFile()
    {
        string filePath = Application.streamingAssetsPath + "/Levels/" + gridFileName; // change gridFileName in the serialized field to specify.
        //Debug.Log(File.ReadAllText(filePath));
        List<string> fileLines = File.ReadAllLines(filePath).ToList(); // Puts the file information into a list of strings (each containing a complete line)
        
        for (int rowIndex = 0; rowIndex < rows; rowIndex++)
        {
            string data = fileLines[rowIndex]; // Grab one line from the list
            for (int colIndex = 0; colIndex < columns; colIndex++)
            {
                if (data[colIndex] == 'x') // Grabs one character index from that line string
                {
                    // Set the blocktype to the text value. 
                    gridArray[rowIndex, colIndex].GetComponent<AStarGridCell>().blockType = AStarGridCell.BlockType.Untraversable;
                }
                // Stub line if we want to care about any other information in the file. We only care about 'x' chars rn since cells are 'o' by default.
                // if (data[colIndex] == 'whatever else') {} 
            }
            
        }        
    }
}
