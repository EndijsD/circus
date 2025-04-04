using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using System.IO;
using static SaveLoad;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;
    public int currentPlayerIndex = 0;
    public List<PlayerMover> players = new List<PlayerMover>();
    public bool isMoving = false;
    public GameObject victoryScreen;
    private GameObject[] playerUIInstances;
    public Player player;
    float animationLength;

    private void Awake()
    {
        victoryScreen.SetActive(false);
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterPlayer(PlayerMover player)
    {
        players.Add(player);
    }

    public void NextTurn()
    {
        if (players.Count == 0) return;
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
    }

    public void TriggerVictory(PlayerMover winner)
    {
        victoryScreen.SetActive(true);
        playerUIInstances = new GameObject[players.Count - 1];

        int i = 0;
        foreach (var extraPlayer in players)
        {
            if(extraPlayer == winner) continue;

            playerUIInstances[i] = Instantiate(player.playerUIPrefabs[extraPlayer.playerIndex], victoryScreen.transform);
            playerUIInstances[i].SetActive(false);
            playerUIInstances[i].transform.localPosition = new Vector3(200, -50, 0);
            playerUIInstances[i].transform.localRotation = Quaternion.Euler(0, 180, 0);
            playerUIInstances[i].transform.Find("NameField").gameObject.SetActive(false);
            playerUIInstances[i].GetComponent<Image>().raycastTarget = false;
            i++;
        }

        GameObject playerUI = Instantiate(player.playerUIPrefabs[winner.playerIndex], victoryScreen.transform);
        playerUI.SetActive(true);
        playerUI.GetComponent<RectTransform>().sizeDelta = new Vector2(500, 500);
        playerUI.GetComponent<Animator>().SetBool("isVictory", true);
        playerUI.GetComponent<Image>().raycastTarget = false;
        TextMeshProUGUI winnerNameText = playerUI.GetComponentInChildren<TextMeshProUGUI>();
        winnerNameText.text = winner.GetComponent<Name>().GetPlayerName();

        SaveVictoryDetails(winner);
        animationLength = playerUI.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length - .9f;
        StartCoroutine(CycleUICharacters(winner));
    }

    [System.Serializable]
    public class SaveData
    {
        public string name;
        public int thorwn;
        public int numberSum;
    }

    private List<SaveData> saveDataList = new List<SaveData>();

    [System.Serializable]
    public class SaveDataArray
    {
        public SaveData[] saveDataArray;
    }

    void SaveVictoryDetails(PlayerMover winner)
    {
        SaveData saveData = new SaveData();
        saveData.name = winner.GetComponent<Name>().GetPlayerName();
        saveData.thorwn = winner.GetComponent<PlayerMover>().diceThorwnCount;
        saveData.numberSum = winner.GetComponent<PlayerMover>().thorwnDiceNumberSum;

        string filePath = Application.persistentDataPath + "/victoryDetails.json";

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            if (!string.IsNullOrEmpty(json))
            {
                SaveDataArray dataArray = JsonUtility.FromJson<SaveDataArray>(json);
                saveDataList = new List<SaveData>(dataArray.saveDataArray);
            }
        }
        else
        {
            saveDataList = new List<SaveData>();
        }

        saveDataList.Add(saveData);

        SaveDataArray saveDataArray = new SaveDataArray();
        saveDataArray.saveDataArray = saveDataList.ToArray();

        string updatedJson = JsonUtility.ToJson(saveDataArray, true);
        File.WriteAllText(filePath, updatedJson);
    }

    IEnumerator CycleUICharacters(PlayerMover winner)
    {
        //List<GameObject> filteredUIInstances = new List<GameObject>();

        //for (int i = 0; i < playerUIInstances.Length; i++)
        //{
        //    if (i != winner.playerIndex)
        //        filteredUIInstances.Add(playerUIInstances[i]);
        //    else
        //        playerUIInstances[i].SetActive(false);
        //}

        if (playerUIInstances.Length == 0) yield break;

        int cycleIndex = 0;
        while (true)
        {
            for (int i = 0; i < playerUIInstances.Length; i++)
            {
                playerUIInstances[i].SetActive(i == cycleIndex);

                if (playerUIInstances[i].transform.eulerAngles.x <= 70)
                    playerUIInstances[i].transform.Rotate(5, 0, 0);

                Vector3 position = playerUIInstances[i].transform.localPosition;
                position.y = Mathf.Max(position.y - 5, -110);
                playerUIInstances[i].transform.localPosition = position;
            }

            yield return new WaitForSeconds(animationLength);
            cycleIndex = (cycleIndex + 1) % playerUIInstances.Length;
        }

        //    List<GameObject> filteredUIInstances = new List<GameObject>();

        //    for (int i = 0; i < playerUIInstances.Length; i++)
        //    {
        //        if (i != winner.playerIndex)
        //            filteredUIInstances.Add(playerUIInstances[i]);
        //        else
        //            playerUIInstances[i].SetActive(false);
        //    }

        //    if (filteredUIInstances.Count == 0) yield break;

        //    int cycleIndex = 0;
        //    while (true)
        //    {
        //        for (int i = 0; i < filteredUIInstances.Count; i++)
        //        {
        //            filteredUIInstances[i].SetActive(i == cycleIndex);

        //            if (filteredUIInstances[i].transform.eulerAngles.x <= 70)
        //                filteredUIInstances[i].transform.Rotate(5, 0, 0);

        //            Vector3 position = filteredUIInstances[i].transform.localPosition;
        //            position.y = Mathf.Max(position.y - 5, -110);
        //            filteredUIInstances[i].transform.localPosition = position;
        //        }

        //        yield return new WaitForSeconds(animationLength);
        //        cycleIndex = (cycleIndex + 1) % filteredUIInstances.Count;
        //    }
    }

}
