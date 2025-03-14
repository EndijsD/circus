using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    Image img;
    Color tempColor;

    void Start()
    {
        img = GetComponent<Image>();
        tempColor = img.color;
        tempColor.a = 1f;
        img.color = tempColor;
        StartCoroutine(FadeIn(0.1f));
    }

    IEnumerator FadeIn(float seconds)
    {
        for (float i = 1f; i >= -0.05f; i -= 0.05f)
        {
            tempColor = img.color;
            tempColor.a = i;
            img.color = tempColor;
            yield return new WaitForSecondsRealtime(seconds);
        }
        img.raycastTarget = false;
    }

    public IEnumerator FadeOut(float seconds)
    {
        img.raycastTarget = true;

        for (float i = 0.05f; i <= 1.05f; i += 0.05f)
        {
            tempColor = img.color;
            tempColor.a = i;
            img.color = tempColor;
            yield return new WaitForSecondsRealtime(seconds);
        }
    }
}
