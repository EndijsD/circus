using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField]
    private AudioMixer audioMixer;
    [SerializeField]
    private Slider audioSlider;

    [SerializeField]
    private TMP_Dropdown resolutionsDropdown;
    private List<string> resolutionsList = new List<string>();
    

    [SerializeField]
    private TextMeshProUGUI lowText, mediumText, highText;

    [SerializeField]
    private TextMeshProUGUI fpsText;

    private void Awake()
    {
        // For debugging
        //  Screen.SetResolution(1920, 1080, Screen.fullScreen);

        resolutionsDropdown.ClearOptions();

        //foreach (Resolution rez in Screen.resolutions)
        //{
        //    Debug.Log(rez);
        //    Debug.Log(Screen.currentResolution);
        //    if (Screen.currentResolution.refreshRateRatio.Equals(rez.refreshRateRatio))
        //    {
        //        string modifiedRez = rez.ToString();
        //        modifiedRez = modifiedRez.Substring(0, modifiedRez.IndexOf('@') - 1);
        //        resolutionsList.Add(modifiedRez);
        //    }
        //}
        HashSet<string> uniqueResolutions = new HashSet<string>();

        foreach (Resolution rez in Screen.resolutions)
        {
            string modifiedRez = rez.ToString();
            modifiedRez = modifiedRez.Substring(0, modifiedRez.IndexOf('@') - 1);
            uniqueResolutions.Add(modifiedRez);
        }

        resolutionsList = uniqueResolutions.ToList();
        resolutionsList.Reverse();

        resolutionsDropdown.AddOptions(resolutionsList);
    }

    public void ChangeResolution()
    {
        string fullRez = resolutionsList[resolutionsDropdown.value].ToString();

        string width = fullRez.Substring(0, fullRez.IndexOf(' '));
        string height = fullRez.Substring(fullRez.IndexOf('x') + 2, fullRez.Length - (fullRez.IndexOf('x') + 2));

        //Debug.Log("Resolution width: " + width);
        //Debug.Log("Resolution height: " + height);

        Screen.SetResolution(System.Int32.Parse(width), System.Int32.Parse(height), Screen.fullScreen);
    }

    public void ToggleFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        //Debug.Log("Fullscreen: " + isFullscreen);
    }

    public void ChangeQuality(Slider slider)
    {
        int qualityIndex = (int)slider.value;

        switch (qualityIndex)
        {
            case 0:
                lowText.alpha = 1f;
                mediumText.alpha = 0.4f;
                highText.alpha = 0.4f;
                QualitySettings.SetQualityLevel(qualityIndex);
                break;

            case 1:
                lowText.alpha = 0.4f;
                mediumText.alpha = 1f;
                highText.alpha = 0.4f;
                QualitySettings.SetQualityLevel(qualityIndex);
                break;

            case 2:
                lowText.alpha = 0.4f;
                mediumText.alpha = 0.4f;
                highText.alpha = 1f;
                QualitySettings.SetQualityLevel(qualityIndex);
                break;
        }
        //Debug.Log("Quality: " + value);
    }

    public void ChangeFPS(Slider slider)
    {
        int FPS = (int)slider.value;

        if (FPS == 501)
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = -1;
            fpsText.text = "Unlimited";
        }
        else if (FPS < 501 && FPS > 10)
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = FPS;
            fpsText.text = FPS.ToString();
        }
        else
        {
            QualitySettings.vSyncCount = 1;
            fpsText.text = "vSync";
        }
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
    }
}
