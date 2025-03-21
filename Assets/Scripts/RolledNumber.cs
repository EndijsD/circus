using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RolledNumber : MonoBehaviour
{
    DiceRoll diceRoll;
    //[SerializeField]
    //Text rolledNumberText;
    [SerializeField]
    private TextMeshProUGUI rolledNumberText;

    void Awake()
    {
        diceRoll = FindObjectOfType<DiceRoll>();
    }

    void Update()
    {
        if(diceRoll != null)
        {
            if (diceRoll.hasLanded)
                rolledNumberText.text = diceRoll.diceFaceNum;
            else rolledNumberText.text = "?";
        }
        else
        {
            Debug.Log("DiceRoll not found in a scene!");
        }
    }
}
