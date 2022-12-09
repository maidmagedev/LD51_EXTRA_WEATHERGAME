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
    // check this path to generate a path
    public bool testPath = false;
    // first point
    public int x1;
    public int y1;
    // second point
    public int x2;
    public int y2;
    private List<AStarGridCell> prevPath;


    [Header("Editor Tools")]
    public bool blockOutMode = false; // used by TileDetectMouse.cs to determine if clicking on tiles will mark tiles as traversable/untraversable.
                                      // TileDetectMouse.cs has a reference of this script.

    public bool mouseOnHoverPathBuilding = false; // with this set to true, paths will be continuously generated to the mouse position.
                                                  // used by TileDetectMouse.cs. Will do nothing otherwise.

    private bool traversableShow = false;

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

    public void ToggleUntraversableTileView()
    {
        Debug.Log("Hello!!!");
        if (traversableShow == false)
        {
            HideUntraversableTiles();

        }
        else
        {
            ShowUntraversableTiles();
        }
        traversableShow = !traversableShow;
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
    
    public void HideUntraversableTiles()
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
                    gridCell.transform.GetChild(4).gameObject.SetActive(false);
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
                    gridArray[colIndex, rowIndex].GetComponent<AStarGridCell>().blockType = AStarGridCell.BlockType.Untraversable;
                }
                // Stub line if we want to care about any other information in the file. We only care about 'x' chars rn since cells are 'o' by default.
                // if (data[colIndex] == 'whatever else') {} 
            }

        }
    }

    // Finds a path between two grid cells.
    // Returns an ordered list of the path.

    // There's currently a bias to tread over previous paths when called consecutively. This bias results in a lot of valid paths returning false.
    public List<AStarGridCell> GetPath(AStarGridCell start, AStarGridCell end)
    {
        Debug.Log("[--------------------------------------GetPath() Start--------------------------------------]");
        // safety exit
        if (start.blockType == AStarGridCell.BlockType.Untraversable || end.blockType == AStarGridCell.BlockType.Untraversable)
        {
            Debug.Log(" ! SGrid.cs in GetPath() method : Either Start or End is of blockType untraversable.");
            return null;
        }
        if (start == end)
        {
            Debug.Log(" ! SGrid.cs in GetPath() method : Start is same as end.");
            return null;
        }
        if (prevPath != null)
        {
            clearPath(prevPath);
        }

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
        Debug.Log("?? Queue current size: " + queue.size());

        while (!queue.isEmpty() && !targetFound)
        {
            AStarGridCell currCellData = queue.findMin();
            queue.deleteMin();
            Debug.Log("Current Cell: (" + currCellData.x + ", " + currCellData.y + ")");

            currCellData.visited = true;
            visited.Add(currCellData);

            // Display Debug Text ---------------------------------------------------------------
            /*
            gridArray[currCellData.x, currCellData.y].transform.GetChild(1).gameObject.SetActive(true);
            string text = "G: ";
            text += currCellData.gCost;
            text += "\nF: ";
            text += currCellData.fCost;
            gridArray[currCellData.x, currCellData.y].transform.GetChild(1).GetComponent<TextMeshPro>().text = text;*/


            if (currCellData == end)
            {
                targetFound = true;
                continue;
            }

            // Get neighboring tiles
            formNeighbors(currCellData);

            Debug.Log("Current cell has " + currCellData.neighbors.Count + " neighbors.");

            // Get (fcost) distance of neighbors, if it's lower than known, overwrite it.
            foreach (AStarGridCell neighborCellData in currCellData.neighbors)
            {
                if (!neighborCellData.visited && neighborCellData.blockType == AStarGridCell.BlockType.Traversable)
                {
                    int cost = currCellData.fCost + neighborCellData.weight;
                    if (cost < neighborCellData.fCost)
                    {
                        neighborCellData.gCost = cost;
                        neighborCellData.fCost = neighborCellData.CalculateFCost();
                        neighborCellData.parent = currCellData;

                        queue.insert(neighborCellData);
                        Debug.Log(" - neighbor: (" + neighborCellData.x + ", " + neighborCellData.y + ") added to the queue");
                    } else
                    {
                        Debug.Log(" - neighbor: (" + neighborCellData.x + ", " + neighborCellData.y + ") Not added to the queue, size is lesser.");
                    }
                } else
                {
                    Debug.Log(" - neighbor: (" + neighborCellData.x + ", " + neighborCellData.y + ") NOT Traversable, or was Visited");
                }
            }

            Debug.Log("/////// Queue size going into next iteration: " + queue.size());
        }


        // Found the Destination
        if (targetFound)
        {
            AStarGridCell curr = end;
            while (curr != null)
            {
                Debug.Log("path : (" + curr.x + ", " + curr.y + ")");
                finalPath.Add(curr);
                gridArray[curr.x, curr.y].transform.GetChild(5).gameObject.SetActive(true);

                curr = curr.parent;
            }
            gridArray[start.x, start.y].transform.GetChild(1).gameObject.GetComponent<TextMeshPro>().text = "START";
            gridArray[start.x, start.y].transform.GetChild(1).gameObject.SetActive(true);
            gridArray[end.x, end.y].transform.GetChild(1).gameObject.GetComponent<TextMeshPro>().text = "END";
            gridArray[end.x, end.y].transform.GetChild(1).gameObject.SetActive(true);

        }
        else
        {
            Debug.Log("Target not found");
        }

        // reset all as unvisited
        foreach (AStarGridCell item in visited)
        {
            item.visited = false;
            item.ClearCosts();
            item.parent = null;
        }

        while (!queue.isEmpty())
        {
            // clean the queue, these were never visited.
            AStarGridCell temp = queue.findMin();
            Debug.Log("Queue Dump: (" + temp.x + ", " + temp.y + ")");
            temp.ClearCosts();
            temp.parent = null;
            queue.deleteMin();
        }
        awfulClean();

        Debug.Log("[--------------------------------------GetPath()--end---------------------------------------]");
        prevPath = finalPath;
        return finalPath;
    }

    // Helper method for GetPath(). Returns neighboring North, South, East and West cells.
    private void formNeighbors(AStarGridCell currCellData)
    {
        currCellData.neighbors = new();
        if (currCellData.x - 1 >= 0)
        {
            // west
            //Debug.Log(" // West ok.");
            GameObject west = gridArray[currCellData.x - 1, currCellData.y];
            //west.transform.GetChild(1).gameObject.SetActive(true);
            //west.transform.GetChild(1).gameObject.GetComponent<TextMeshPro>().text = "WWWWWWWWW";

            currCellData.neighbors.Add(west.GetComponent<AStarGridCell>());
        }
        else
        {
            Debug.Log(" // West is out of bounds.");
        }
        if (currCellData.x + 1 < columns)
        {
            // east
            //Debug.Log(" // east ok.");
            GameObject east = gridArray[currCellData.x + 1, currCellData.y];
            //east.transform.GetChild(1).gameObject.SetActive(true);
            //east.transform.GetChild(1).gameObject.GetComponent<TextMeshPro>().text = "EEEEEEEEEEEEEEE";

            currCellData.neighbors.Add(east.GetComponent<AStarGridCell>());

        }
        else
        {
            Debug.Log(" // East is out of bounds.");
        }
        if (currCellData.y - 1 >= 0)
        {
            // south
            //Debug.Log(" // south ok");

            GameObject south = gridArray[currCellData.x, currCellData.y - 1];
            //south.transform.GetChild(1).gameObject.SetActive(true);
            //south.transform.GetChild(1).GetComponent<TextMeshPro>().text = "SSSSSSSSSSSSSSS";

            currCellData.neighbors.Add(south.GetComponent<AStarGridCell>());

        }
        else
        {
            Debug.Log(" // South is out of bounds.");
        }
        if (currCellData.y + 1 < rows)
        {
            // north
            //Debug.Log(" // north ok");
            GameObject north = gridArray[currCellData.x, currCellData.y + 1];
            //north.transform.GetChild(1).gameObject.SetActive(true);
            //north.transform.GetChild(1).gameObject.GetComponent<TextMeshPro>().text = "NNNNNNNNNNNN";

            currCellData.neighbors.Add(north.GetComponent<AStarGridCell>());

        }
        else
        {
            Debug.Log(" // North is out of bounds.");
        }
    }


    public void clearPath(List<AStarGridCell> path)
    {
        foreach (AStarGridCell cell in path)
        {
            gridArray[cell.x, cell.y].transform.GetChild(5).gameObject.SetActive(false);
            gridArray[cell.x, cell.y].transform.GetChild(1).gameObject.SetActive(false);
            gridArray[cell.x, cell.y].transform.GetChild(6).gameObject.SetActive(false);

        }
    }

    enum PathDir
    {

    }
        

    public void InterpretPath(List<AStarGridCell> path, PlayerMovement player)
    {
        if (path != null && path.Count > 1)
        {
            Debug.Log("InterpretPath Called with path length: " + path.Count);

            path.Reverse();
            AStarGridCell prev = path.ElementAt(0);
            // Path is guaranteed to contain at least two items.
            int lastDir = -1; // -1 no dir | 0 horizontal (X) | 1 vertical (Y)
            int currentDiff = 0;
            for (int i = 1; i < path.Count; i++)
            {
                AStarGridCell curr = path.ElementAt(1);
                int xDiff = curr.x - prev.x;
                int yDiff = curr.y - prev.y;
                
              
                switch(lastDir)
                {
                    case -1:
                        if (xDiff != 0)
                        {
                            lastDir = 1;
                        } else
                        {
                            lastDir = 0;
                        }
                        
                        break;
                    case 0:
                        if (yDiff != 0)
                        {
                            // There is a difference in y, meaning we have a change of direction.
                            Debug.Log("Move along X " + currentDiff + " # tiles");
                            // Now we move along the Y axis.
                            lastDir = 1;
                            currentDiff = 0;
                        } else
                        {

                        }
                        break;
                    case 1:
                        if (xDiff != 0)
                        {
                            // There is a difference in x, meaning we have a change of direction.
                            Debug.Log("Move along Y" + currentDiff + " # tiles");
                            // Now we will move along the X axis.
                            lastDir = 0;
                            currentDiff = 0;
                        }
                        break;
                }
                currentDiff += xDiff + yDiff;
            }
            Debug.Log("End Diff: " + currentDiff);

        }
        else
        {
            Debug.Log("Path should never have less than two items if non-null.");
        }


    }

    public void awfulClean()
    {
        for (int rowIndex = 0; rowIndex < rows; rowIndex++)
        {
            for (int colIndex = 0; colIndex < columns; colIndex++)
            {

                AStarGridCell cell = gridArray[colIndex, rowIndex].GetComponent<AStarGridCell>();
                cell.ClearCosts();
                
            }
        }
    }
}
