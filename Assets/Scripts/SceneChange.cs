using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public Fade fadeScript;
    public SaveLoad saveLoad;

    public void CloseGame()
    {
        StartCoroutine(Delay("quit", -1, ""));
    }

    public IEnumerator Delay(string command, int character, string name)
    {
        yield return fadeScript.FadeOut(0.1f);

        if (string.Equals(command, "quit", System.StringComparison.OrdinalIgnoreCase))
        {
            PlayerPrefs.DeleteAll();

            if (UnityEditor.EditorApplication.isPlaying)
                UnityEditor.EditorApplication.isPlaying = false;
            else
                Application.Quit();
        }
        else if (string.Equals(command, "play", System.StringComparison.OrdinalIgnoreCase))
        {
            saveLoad.SaveGame(character, name);
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
    }
}
