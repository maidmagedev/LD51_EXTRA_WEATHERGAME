using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridDebugDisplay : MonoBehaviour
{
    public TextMeshPro textMesh;
    public GridStat gridStat;

    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponent<TextMeshPro>();
        gridStat = GetComponentInParent<GridStat>();
    }

    // Update is called once per frame
    void Update()
    {
        textMesh.text = "v: " + gridStat.visited + "\n" + gridStat.x + ", " + gridStat.y;
    }
}
