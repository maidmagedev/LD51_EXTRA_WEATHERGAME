using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using TMPro;

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

    [Header("Manual Path Debugging")]
    public bool testPath = false;
    public int x1;
    public int y1;
    public int x2;
    public int y2;

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

        if (testPath)
        {
            AStarGridCell initial = gridArray[x1, y1].GetComponent<AStarGridCell>();
            AStarGridCell final = gridArray[x2, y2].GetComponent<AStarGridCell>();
            GetPath(initial, final);
            Debug.Log("Path finished.");
            testPath = false;
        }
    }

    // Enables all valid "untraversable" indicators.
    public void ShowUntraversableTiles()
    {
        for (int rowIndex = 0; rowIndex < rows; rowIndex++)
        {
            for (int colIndex = 0; colIndex < columns; colIndex++)
            {
                GameObject gridCell = gridArray[colIndex, rowIndex];
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

                // gridCell contains information of it's index position in the grid.
                AStarGridCell cellData = gridCell.GetComponent<AStarGridCell>();

                cellData.x = colIndex;
                cellData.y = rowIndex;

                gridCell.transform.SetParent(gameObject.transform); // Sets this created cell object as a child of this scriptholder
                gridArray[colIndex, rowIndex] = gridCell; // Grid array stores the representations of each cell.
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

    // Finds a path between two grid cells.
    // Returns an ordered list of the path.
    public List<AStarGridCell> GetPath(AStarGridCell start, AStarGridCell end)
    {
        List<AStarGridCell> finalPath = new();
        List<AStarGridCell> visited = new(); // we use this for cleanup, rather than in our methods. Looking for visited.contains() is redundant.

        bool targetFound = false;

        //PriorityQueue<> PQ = new PriorityQueue<>();
        PrioQueue queue = new(rows * columns);

        start.visited = true;
        start.gCost = 0;
        start.fCost = start.CalculateFCost();
        visited.Add(start);


        queue.insert(start);
        Debug.Log("inserting: (" + start.x + ", " + start.y + ")");
        while (!queue.isEmpty() && !targetFound)
        {
            AStarGridCell currCellData = queue.findMin();
            queue.deleteMin();
            Debug.Log("Current Cell: (" + currCellData.x + ", " + currCellData.y + ")");
            currCellData.visited = true;
            visited.Add(currCellData);
            gridArray[currCellData.x, currCellData.y].transform.GetChild(1).gameObject.SetActive(true);
            string text = "G: ";
            text += currCellData.gCost;
            text += "\nF: ";
            text += currCellData.fCost;
            gridArray[currCellData.x, currCellData.y].transform.GetChild(1).GetComponent<TextMeshPro>().text = text;


            if (currCellData == end)
            {
                targetFound = true;
                continue;
            }

            // Get the gridArray object at that position

            // Get neighboring tiles
            
            currCellData.neighbors = new();
            if (currCellData.x - 1 >= 0)
            {
                // west
                Debug.Log(" // West ok.");
                GameObject west = gridArray[currCellData.x - 1, currCellData.y];
                //west.transform.GetChild(1).gameObject.SetActive(true);
                //west.transform.GetChild(1).gameObject.GetComponent<TextMeshPro>().text = "WWWWWWWWW";

                currCellData.neighbors.Add(west.GetComponent<AStarGridCell>());
            } else
            {
                Debug.Log(" // West is null.");
            }
            if (currCellData.x + 1 < rows)
            {
                // east
                Debug.Log(" // east ok.");
                GameObject east = gridArray[currCellData.x + 1, currCellData.y];
                //east.transform.GetChild(1).gameObject.SetActive(true);
                //east.transform.GetChild(1).gameObject.GetComponent<TextMeshPro>().text = "EEEEEEEEEEEEEEE";

                currCellData.neighbors.Add(east.GetComponent<AStarGridCell>());

            }
            else
            {
                Debug.Log(" // East is null.");
            }
            if (currCellData.y - 1 >= 0)
            {
                // south
                Debug.Log(" // south ok");
                
                GameObject south = gridArray[currCellData.x, currCellData.y - 1];
                //south.transform.GetChild(1).gameObject.SetActive(true);
                //south.transform.GetChild(1).GetComponent<TextMeshPro>().text = "SSSSSSSSSSSSSSS";
                
                currCellData.neighbors.Add(south.GetComponent<AStarGridCell>());

            }
            else
            {
                Debug.Log(" // South is null.");
            }
            if (currCellData.y + 1 < columns)
            {
                // north
                Debug.Log(" // north ok");
                GameObject north = gridArray[currCellData.x, currCellData.y + 1];
                //north.transform.GetChild(1).gameObject.SetActive(true);
                //north.transform.GetChild(1).gameObject.GetComponent<TextMeshPro>().text = "NNNNNNNNNNNN";

                currCellData.neighbors.Add(north.GetComponent<AStarGridCell>());

            }
            else
            {
                Debug.Log(" // North is null.");
            }

            foreach (AStarGridCell neighborCellData in currCellData.neighbors)
            {
                if (!neighborCellData.visited && neighborCellData.blockType == AStarGridCell.BlockType.Traversable)
                {
                    Debug.Log(" - neighbor: (" + neighborCellData.x + ", " + neighborCellData.y + ") added to the queue");
                    int cost = currCellData.fCost + neighborCellData.weight;
                    if (cost < neighborCellData.fCost)
                    {
                        neighborCellData.gCost = cost;
                        neighborCellData.fCost = neighborCellData.CalculateFCost();
                        neighborCellData.parent = currCellData;

                        queue.insert(neighborCellData);
                    }
                } else
                {
                    Debug.Log(" - neighbor: (" + neighborCellData.x + ", " + neighborCellData.y + ") NOT added to the queue");
                }
            }

            Debug.Log("/////// Queue size going into next iteration: " + queue.size());
        }



        // reset all as unvisited
        foreach (AStarGridCell item in visited)
        {
            item.visited = false;
            item.ClearCosts();
        }

        if (targetFound)
        {
            AStarGridCell curr = end;
            int i = 0;
            while (curr != null)
            {
                Debug.Log("path : (" + curr.x + ", " + curr.y + ")");
                finalPath.Add(curr);
                gridArray[curr.x, curr.y].transform.GetChild(3).gameObject.SetActive(true);
                string text = "path ";
                text += i;
                //gridArray[curr.x, curr.y].transform.GetChild(1).gameObject.GetComponent<TextMeshPro>().text = text;
                curr = curr.parent;
                i++;
            }
            gridArray[start.x, start.y].transform.GetChild(1).gameObject.GetComponent<TextMeshPro>().text = "START";
            gridArray[end.x, end.y].transform.GetChild(1).gameObject.GetComponent<TextMeshPro>().text = "END";

        }
        else
        {
            Debug.Log("Target not found");
        }

        return finalPath;
    }
}
