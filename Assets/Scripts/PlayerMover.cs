using System;
using System.Collections;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    private BoardCreator boardCreator;
    private DiceRoll diceRoll;
    private int positionIndex = 0;
    private Vector3 positionOffset;
    public Animator animator;
    public int playerIndex;
    public int diceThorwnCount = 0;
    public int thorwnDiceNumberSum = 0;
    private bool speedUp = false;

    void Start()
    {
        diceRoll = FindObjectOfType<DiceRoll>();
        boardCreator = FindObjectOfType<BoardCreator>();
        animator = gameObject.GetComponent<Animator>();

        if (boardCreator != null && boardCreator.spawnedPlatforms.Count > 0)
            positionOffset = transform.position - boardCreator.spawnedPlatforms[0].transform.position;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            speedUp = !speedUp;

        for (int i = 1; i <= 9; i++)
            if (!TurnManager.Instance.isMoving && Input.GetKeyDown(KeyCode.Alpha1 + (i - 1)) && TurnManager.Instance.players[TurnManager.Instance.currentPlayerIndex] == this)
                StartCoroutine(MoveToPosition(i));


            if (!TurnManager.Instance.isMoving && diceRoll != null && diceRoll.hasLanded)
        {
            int diceResult = Convert.ToInt32(diceRoll.diceFaceNum);
            
            if (diceResult > 0 && TurnManager.Instance.players[TurnManager.Instance.currentPlayerIndex] == this)
            {
                diceThorwnCount++;
                thorwnDiceNumberSum += diceResult;
                StartCoroutine(MoveToPosition(diceResult));
                diceRoll.diceFaceNum = "0";
            }
        }
    }

    IEnumerator MoveToPosition(int steps)
    {
        TurnManager.Instance.isMoving = true;
        animator.SetBool("isWalking", true);

        int maxIndex = boardCreator.spawnedPlatforms.Count - 1;
        bool movingForward = true;

        for (int i = 0; i < steps; i++)
        {
            if (movingForward)
            {
                positionIndex++;
                if (positionIndex > maxIndex)
                {
                    movingForward = false;
                    positionIndex = maxIndex - 1;
                }
            }
            else
            {
                positionIndex--;
            }

            Vector3 currentPos = transform.position;
            Vector3 nextPos = boardCreator.spawnedPlatforms[positionIndex].transform.position + positionOffset;

            if (nextPos.x > currentPos.x)
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            else if (nextPos.x < currentPos.x)
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            float elapsedTime = 0f;
            float duration = speedUp ? 0f : 0.5f;
            Vector3 startPos = transform.position;

            while (elapsedTime < duration)
            {
                transform.position = Vector3.Lerp(startPos, nextPos, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = nextPos;
            yield return new WaitForSeconds(0.2f);
        }

        int newPositionIndex = boardCreator.CheckForLadder(positionIndex);
        bool isLadder = newPositionIndex != positionIndex;

        if (isLadder)
        {
            Debug.Log($"Ladder found! Moving from {positionIndex} to {newPositionIndex}");
            positionIndex = newPositionIndex;
            yield return StartCoroutine(ClimbLadder(newPositionIndex));
        }

        if (positionIndex == maxIndex)
        {
            TurnManager.Instance.TriggerVictory(this);
        }

        animator.SetBool("isWalking", false);
        TurnManager.Instance.NextTurn();
        diceRoll.Initialize(true);
        TurnManager.Instance.isMoving = false;
    }

    IEnumerator ClimbLadder(int targetIndex)
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = boardCreator.spawnedPlatforms[targetIndex].transform.position + positionOffset;

        float elapsedTime = 0f;
        float climbDuration = speedUp ? 0f : 1.5f;

        while (elapsedTime < climbDuration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / climbDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        positionIndex = targetIndex;
    }
}
