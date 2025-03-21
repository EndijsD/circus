using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject[] playerPrefabs;
    int characterIndex;
    public GameObject spawnPoint;
    int[] otherPlayers;
    int index;
    private const string textFileName = "playerNames";

    void Start()

    {
        characterIndex = PlayerPrefs.GetInt("SelectedCharacter");
        GameObject mainCharacter = Instantiate(playerPrefabs[characterIndex], spawnPoint.transform.position, spawnPoint.transform.rotation);
        mainCharacter.GetComponent<Name>().SetPlayerName(PlayerPrefs.GetString("PlayerName"));

        otherPlayers = new int[PlayerPrefs.GetInt("PlayerCount")];
        string[] newArray = ReadLineFromFile(textFileName);

        for (int i = 0; i < otherPlayers.Length-1; i++)
        {
            spawnPoint.transform.position += new Vector3(4f, 0, 0.08f);
            index = Random.Range(0, playerPrefabs.Length);
            GameObject otherCharacter = Instantiate(playerPrefabs[index], spawnPoint.transform.position, spawnPoint.transform.rotation);
            otherCharacter.GetComponent<Name>().SetPlayerName(newArray[Random.Range(0, newArray.Length)]);
        }
    }

    string[] ReadLineFromFile(string fileName)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(fileName);
        if (textAsset != null)
        {
            return textAsset.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries );
        }
        else
        {
            Debug.LogError("File not found: " + fileName);
            return new string[0];
        }
    }
}
