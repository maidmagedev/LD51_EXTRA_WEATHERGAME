using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SGrid : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] GameObject pfGrid; // grid prefab
    [SerializeField] int rows = 10;
    [SerializeField] int columns = 10;

    [Header("Grid")]
    public GameObject[,] gridArray;


    private void Awake()
    {
        if (pfGrid)
        {

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
