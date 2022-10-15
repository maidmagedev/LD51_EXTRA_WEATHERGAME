using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Based heavily on the tutorial by YT@CodeMonkey.
// https://www.youtube.com/watch?v=waEsGu--9P8&list=PLzDRvYVwl53uhO8yhqxcyjDImRjO9W722&index=1&t=408s&ab_channel=CodeMonkey

public class CMGrid
{
    private int width;
    private int height;
    private float cellSize; // I think im going to remove this later.
    private int[,] gridArray;
    private TextMesh[,] debugTextArray;

    public CMGrid(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        gridArray = new int[width, height];
        this.cellSize = cellSize;
        debugTextArray = new TextMesh[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y< gridArray.GetLength(1); y++)
            {
                //Debug.Log("(" + x + ", " + y + ")");
                debugTextArray[x, y] = CreateWorldText(gridArray[x, y].ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f, 20, Color.white, TextAnchor.MiddleCenter);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);

            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize;
    }

    // Grabs the X & Y of a worldPosition as output integers..
    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt(worldPosition.x / cellSize);
        y = Mathf.FloorToInt(worldPosition.z / cellSize);
    }

    // I think this will be removed.
    public void SetValue(int x, int y, int value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            debugTextArray[x, y].text = gridArray[x, y].ToString();
        }
        else
        {
            Debug.Log("grid.cs : SetValue() - was provided an invalid x or y value: " + x + ", " + y);
        }
    }

    // I think this will be removed.
    public void SetValue(Vector3 worldPosition, int value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }


    // Think I'm going to remove this later
    public static TextMesh CreateWorldText(string text, Transform parent, Vector3 localposition, int fontSize, Color color, TextAnchor textAnchor)
    {
        GameObject gameObj = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObj.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localposition;
        TextMesh textMesh = gameObj.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        //textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }


}
