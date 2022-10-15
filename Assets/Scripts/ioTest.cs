using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class ioTest : MonoBehaviour
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

        for (int i = 0; i < grid.gridArray.GetLength(0); i++)
        {
            for (int j = 0; j < grid.gridArray.GetLength(1); j++)
            {
                if (grid.gridArray[i, j].GetComponent<AStarGridCell>().blockType == AStarGridCell.BlockType.Traversable)
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
