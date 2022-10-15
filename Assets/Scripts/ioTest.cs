using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class ioTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CreateText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateText()
    {
        // File path
        string path = Application.dataPath + "/Log.text";
        Debug.Log("testa");

        // Create a file
        if (!File.Exists(path))
        {
            Debug.Log("testb");
            File.WriteAllText(path, "Hello!");
        }

        // content
        string content = "Test TEst TEST new text";

        // add text
        File.WriteAllText(path, content);
    }
}
