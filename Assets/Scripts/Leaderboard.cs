using UnityEngine;
using System.IO;
using TMPro;

public class Leaderboard : MonoBehaviour
{
    public GameObject entry;
    public GameObject container;
    private float yOffset = -100f;

    [System.Serializable]
    public class SaveData
    {
        public string name;
        public int thorwn;
        public int numberSum;
    }

    [System.Serializable]
    public class SaveDataArray
    {
        public SaveData[] saveDataArray;
    }

    void Start()
    {
        string filePath = Application.persistentDataPath + "/victoryDetails.json";

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            if (!string.IsNullOrEmpty(json))
            {
                SaveDataArray dataArray = JsonUtility.FromJson<SaveDataArray>(json);

                foreach (var data in dataArray.saveDataArray)
                    SpawnVictoryEntry(data);

                AdjustContentSize(dataArray.saveDataArray.Length);
            }
            else
                Debug.Log("No data found in the file.");
        }
        else
            Debug.Log("File not found at: " + filePath);
    }

    void SpawnVictoryEntry(SaveData data)
    {
        GameObject entryObject = Instantiate(entry, container.transform);

        RectTransform rectTransform = entryObject.GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, yOffset, rectTransform.localPosition.z);

        TextMeshProUGUI nameText = entryObject.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
        nameText.color = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255);
        TextMeshProUGUI thrownText = entryObject.transform.Find("ThrownText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI numberSumText = entryObject.transform.Find("NumberSumText").GetComponent<TextMeshProUGUI>();

        nameText.text = data.name;
        thrownText.text = data.thorwn.ToString();
        numberSumText.text = data.numberSum.ToString();

        yOffset -= 100f;
    }

    void AdjustContentSize(int entryCount)
    {
        RectTransform contentRect = container.GetComponent<RectTransform>();

        float totalHeight = 100f * entryCount + 100f;

        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, totalHeight);
    }
}
