using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceAnimation : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void RollDice()
    {
        animator.SetBool("isRolling", true);
    }

    public void StopRollDice()
    {
        animator.SetBool("isRolling", false);
    }
}
