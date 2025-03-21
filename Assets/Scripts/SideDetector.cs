using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideDetector : MonoBehaviour
{
    DiceRoll diceRoll;

    void Awake()
    {
        diceRoll = FindObjectOfType<DiceRoll>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(diceRoll != null)
            if(diceRoll.GetComponent<Rigidbody>().velocity == Vector3.zero)
            {
                diceRoll.hasLanded = true;
                diceRoll.diceFaceNum = other.name;
            }
            else
                diceRoll.hasLanded = false;

        else Debug.Log("DiceRoll not found in a scene!");
    }
}
