using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    public AudioClip[] soundEffects;
    public AudioSource effectsSource;
    public void Hover()
    {
        effectsSource.PlayOneShot(soundEffects[0]);
    }

    public void Clicked()
    {
        effectsSource.PlayOneShot(soundEffects[1]);
    }

    public void onDice()
    {
        effectsSource.loop = true;
        effectsSource.clip = soundEffects[2];
        effectsSource.Play();
    }

    public void CancelButton()
    {
        effectsSource.PlayOneShot(soundEffects[3]);
    }

    public void PlayButton()
    {
        effectsSource.PlayOneShot(soundEffects[4]);
    }

    public void NameField()
    {
        effectsSource.PlayOneShot(soundEffects[5]);
    }
}
