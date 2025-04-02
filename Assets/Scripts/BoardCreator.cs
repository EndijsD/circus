using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoardCreator : MonoBehaviour
{
    public int rows = 6;
    public int columns = 8;
    public float gapX = 12;
    public float gapY = 11;
    private int counter = 1;

    public GameObject start;
    public List<GameObject> spawnedPlatforms = new List<GameObject>();

    void Start()
    {
        GenerateBoard();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            RegenerateBoard();
    }

    void GenerateBoard()
    {
        ClearBoard();

        for (int i = 0; i < rows; i++)
        {
            if (i % 2 == 0) // Even rows: left to right
            {
                for (int j = 0; j < columns; j++)
                {
                    CreatePlatform(i, j);
                }
            }
            else // Odd rows: right to left
            {
                for (int j = columns - 1; j >= 0; j--)
                {
                    CreatePlatform(i, j);
                }
            }
        }
    }

    void CreatePlatform(int i, int j)
    {
        Vector3 position = start.transform.position + new Vector3(j * gapX, 0, i * gapY);
        GameObject platform = Instantiate(start);
        platform.transform.position = position;
        platform.transform.rotation = start.transform.rotation;
        platform.transform.localScale = start.transform.localScale;

        TextMeshPro tmp = platform.GetComponentInChildren<TextMeshPro>();
        if (tmp != null)
        {
            tmp.text = (counter++).ToString();
        }
        spawnedPlatforms.Add(platform);
    }

    void ClearBoard()
    {
        foreach (GameObject platform in spawnedPlatforms)
        {
            Destroy(platform);
        }
        spawnedPlatforms.Clear();
    }

    void RegenerateBoard()
    {
        GenerateBoard();
    }
}
