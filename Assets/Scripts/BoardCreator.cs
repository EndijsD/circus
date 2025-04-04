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
    public int arrowCountMin = 6;
    public int arrowCountMax = 10;

    public GameObject start;
    public GameObject arrowPrefab;
    public List<GameObject> spawnedPlatforms = new List<GameObject>();
    private List<GameObject> spawnedArrows = new List<GameObject>();
    private Dictionary<int, int> ladders = new Dictionary<int, int>();

    void Start()
    {
        GenerateBoard();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            GenerateBoard();
    }

    void GenerateBoard()
    {
        ClearBoard();
        ladders.Clear();

        for (int i = 0; i < rows; i++)
        {
            if (i % 2 == 0)
            {
                for (int j = 0; j < columns; j++)
                {
                    CreatePlatform(i, j);
                }
            }
            else
            {
                for (int j = columns - 1; j >= 0; j--)
                {
                    CreatePlatform(i, j);
                }
            }
        }

        GenerateLadders();
    }

    void CreatePlatform(int i, int j)
    {
        Vector3 position = start.transform.position + new Vector3(j * gapX, 0, i * gapY);
        GameObject platform = Instantiate(start, position, start.transform.rotation);
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
        counter = 1;
        foreach (GameObject platform in spawnedPlatforms)
        {
            Destroy(platform);
        }
        foreach (GameObject arrow in spawnedArrows)
        {
            Destroy(arrow);
        }

        spawnedPlatforms.Clear();
        spawnedArrows.Clear();
    }

    void GenerateLadders()
    {
        int ladderCount = Random.Range(arrowCountMin, arrowCountMax);
        HashSet<int> ladderEnds = new HashSet<int>();
        HashSet<int> ladderStarts = new HashSet<int>();

        for (int i = 0; i < ladderCount; i++)
        {
            int startIndex = Random.Range(1, spawnedPlatforms.Count - 1);
            int endIndex;

            bool goUp = Random.value > 0.5f;

            if (goUp)
            {
                endIndex = Random.Range(startIndex + 1, spawnedPlatforms.Count - 1);

                if (ladderEnds.Contains(startIndex) || ladderStarts.Contains(endIndex))
                    continue;
            }
            else
            {
                endIndex = Random.Range(1, startIndex);

                if (ladderStarts.Contains(startIndex) || ladderStarts.Contains(endIndex) || ladderEnds.Contains(endIndex))
                    continue;
            }

            if (startIndex == endIndex || ladders.ContainsKey(startIndex) || ladders.ContainsValue(startIndex))
                continue;

            ladders[startIndex] = endIndex;
            ladderEnds.Add(endIndex);
            ladderStarts.Add(startIndex);

            Debug.Log($"Ladder from {startIndex} to {endIndex}");

            DrawArrow(startIndex, endIndex);
        }
    }


    void DrawArrow(int startIndex, int endIndex)
    {
        GameObject arrowObj = Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity);
        LineRenderer lineRenderer = arrowObj.GetComponent<LineRenderer>();

        Vector3 startPos = spawnedPlatforms[startIndex].transform.position + new Vector3(0, 1, 0);
        Vector3 endPos = spawnedPlatforms[endIndex].transform.position + new Vector3(0, 1, 0);

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);

        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        Gradient gradient = new Gradient();
        Color arrowColor = (endIndex > startIndex) ? Color.green : Color.red;

        gradient.colorKeys = new GradientColorKey[]
        {
        new(arrowColor, 0f),
        new(arrowColor, 1f)
        };
        gradient.alphaKeys = new GradientAlphaKey[]
        {
        new(1f, 0f),
        new(1f, 1f)
        };

        lineRenderer.colorGradient = gradient;
        spawnedArrows.Add(arrowObj);
    }

    public int CheckForLadder(int positionIndex)
    {
        return ladders.ContainsKey(positionIndex) ? ladders[positionIndex] : positionIndex;
    }
}
