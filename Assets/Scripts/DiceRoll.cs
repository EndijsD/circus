using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoll : MonoBehaviour
{
    Rigidbody rBody;
    Vector3 pos;
    public float maxRadForceVal, startRollingForce;
    float x, y, z;
    public string diceFaceNum;
    public bool hasLanded = false;
    public bool thrown = false;

    void Awake()
    {
        Initialize(false);
    }

    private void Update()
    {
        if(rBody != null)
        {
            if(Input.GetMouseButton(0) && hasLanded || Input.GetMouseButton(0) && !thrown)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider != null && hit.collider.gameObject == this.gameObject)
                    {
                        if(!thrown)
                            thrown = true;

                        if(!TurnManager.Instance.isMoving)
                            RollDice();
                    }
                }
            }
        }
    }

    public void Initialize(bool resetAction)
    {
        if(!resetAction)
        {
            rBody = GetComponent<Rigidbody>();
            pos = transform.position;
        }
        else 
            transform.position = pos;

        rBody.isKinematic = true;
        transform.rotation = new Quaternion(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360), 1);
        thrown = false;
        hasLanded = false;
        diceFaceNum = "";
    }

    private void RollDice()
    {
        rBody.isKinematic = false;
        x = Random.Range(0, maxRadForceVal);
        y = Random.Range(0, maxRadForceVal);
        z = Random.Range(0, maxRadForceVal);
        rBody.AddForce(Vector3.up * Random.Range(800, startRollingForce));
        rBody.AddTorque(x, y, z);
    }
}
