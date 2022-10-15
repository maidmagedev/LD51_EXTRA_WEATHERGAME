using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WavefrontGridDebugDisplay : MonoBehaviour
{
    public TextMeshPro textMesh;
    public WavefrontGridStat gridStat;

    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponent<TextMeshPro>();
        gridStat = GetComponentInParent<WavefrontGridStat>();
    }

    // Update is called once per frame
    void Update()
    {
        textMesh.text = "v: " + gridStat.visited + "\n" + gridStat.x + ", " + gridStat.y;
    }
}
