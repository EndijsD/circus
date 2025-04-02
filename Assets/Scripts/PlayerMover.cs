using System;
using System.Collections;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    private BoardCreator boardCreator;
    private DiceRoll diceRoll;
    private int positionIndex = 0;
    private bool isMoving = false;

    public static int currentPlayerIndex = 0; // Track the current player's turn
    public static int totalPlayers = 0; // Number of players

    private Name[] players;  // Store all Name components (which are attached to player GameObjects)
    private int[] playerDiceValues;  // Store dice values for each player

    void Start()
    {
        diceRoll = FindObjectOfType<DiceRoll>();
        boardCreator = FindObjectOfType<BoardCreator>();

        players = FindObjectsOfType<Name>();  // This finds all Name components attached to player GameObjects
        totalPlayers = players.Length;  // Set totalPlayers count

        playerDiceValues = new int[totalPlayers];  // Array to hold dice values for each player
    }

    void Update()
    {
        if (!isMoving && diceRoll != null && diceRoll.hasLanded && Convert.ToInt32(diceRoll.diceFaceNum) > 0 && currentPlayerIndex < totalPlayers)
        {
            // Set the dice value for the current player
            playerDiceValues[currentPlayerIndex] = Convert.ToInt32(diceRoll.diceFaceNum);

            // Start moving the current player
            StartCoroutine(MoveToPosition(playerDiceValues[currentPlayerIndex]));
            diceRoll.diceFaceNum = "0"; // Reset dice result after reading
        }
    }

    IEnumerator MoveToPosition(int steps)
    {
        isMoving = true;

        // Get the current player's GameObject by accessing their Name component
        GameObject currentPlayer = players[currentPlayerIndex].gameObject;

        for (int i = 0; i < steps; i++)
        {
            positionIndex++;
            if (positionIndex >= boardCreator.spawnedPlatforms.Count)
            {
                positionIndex = boardCreator.spawnedPlatforms.Count - 1; // Stay within bounds
                break;
            }

            Vector3 targetPos = boardCreator.spawnedPlatforms[positionIndex].transform.position + new Vector3(0, 2, 0);
            float elapsedTime = 0f;
            float duration = 0.5f;

            Vector3 startPos = currentPlayer.transform.position;
            while (elapsedTime < duration)
            {
                currentPlayer.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            currentPlayer.transform.position = targetPos;
            yield return new WaitForSeconds(0.2f);
        }

        // After movement, reset the dice and move to the next player
        isMoving = false;
        currentPlayerIndex = (currentPlayerIndex + 1) % totalPlayers; // Move to the next player (loop back after last player)
        diceRoll.Initialize(true); // Reset the dice for the next turn
    }
}
