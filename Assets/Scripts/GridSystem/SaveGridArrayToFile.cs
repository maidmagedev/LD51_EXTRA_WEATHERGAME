using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class SaveGridArrayToFile : MonoBehaviour
{
    [SerializeField] SGrid grid;
    [SerializeField] string saveAsFileName = "Log.text";
    private AStarGridCell nothing;
    public bool doThing = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (doThing)
        {
            CreateText();

        }
    }

    void CreateText()
    {
        // File path
        string path = Application.streamingAssetsPath + "/Levels/" + saveAsFileName;

        // Create a file
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "Hello!");
        }

        // clear text in file
        File.WriteAllText(path, "");
        File.AppendAllText(path, "");

        for (int rows = 0; rows < grid.gridArray.GetLength(1); rows++)
        {
            for (int columns = 0; columns < grid.gridArray.GetLength(0); columns++)
            {
                if (grid.gridArray[columns, rows].GetComponent<AStarGridCell>().blockType == AStarGridCell.BlockType.Traversable)
                {
                    File.AppendAllText(path, "o");
                } else
                {
                    File.AppendAllText(path, "x");
                }

            }
            File.AppendAllText(path, "\n");
        }
    }
}
