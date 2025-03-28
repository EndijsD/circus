using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jiggle : MonoBehaviour
{
    [Range(0f, 1f)]
    public float power = 0.1f;

    [Header ("Position")]
    public bool jigglePosition = true;
    public Vector3 positionAmount ;
    [Range(0, 100)]
    public float positionFrequency = 10;
    float positionTime;

    [Header("Rotation")]
    public bool jiggleRotation = true;
    public Vector3 rotationAmount;
    [Range(0, 100)]
    public float rotationFrequency = 10;
    float rotationTime;

    [Header("Scale")]
    public bool jiggleScale = true;
    public Vector3 scaleAmount = new Vector3(.1f, -.1f, .1f);
    [Range(0, 100)]
    public float scaleFrequency = 10;
    float scaleTime;

    Vector3 initialPosition;
    Quaternion initialRotation;
    Vector3 initialScale;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
        initialScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        //var deltaTime = Time.deltaTime; will be paused if user Time.timeScale = 0
        var deltaTime = Time.unscaledDeltaTime;
        positionTime += deltaTime * positionFrequency;
        rotationTime += deltaTime * rotationFrequency;
        scaleTime += deltaTime * scaleFrequency;

        if (jigglePosition)
            transform.localPosition = initialPosition + positionAmount * Mathf.Sin(positionTime) * power;

        if (jiggleRotation)
            transform.localRotation = initialRotation * Quaternion.Euler(rotationAmount * Mathf.Sin(rotationTime) * power);

        if (jiggleScale)
            transform.localScale = initialScale + scaleAmount * Mathf.Sin(scaleTime) * power;
    }
}
