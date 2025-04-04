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
    private Toggle fullscreenToggle;

    [SerializeField]
    private Slider qualitySlider;
    [SerializeField]
    private TextMeshProUGUI lowText, mediumText, highText;

    [SerializeField]
    private Slider fpsSlider;
    [SerializeField]
    private TextMeshProUGUI fpsText;

    private void Awake()
    {
        audioSlider.value = PlayerPrefs.GetFloat("Volume", 0.5f);
        SetVolume(audioSlider.value);
        fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        ToggleFullscreen();
        qualitySlider.value = PlayerPrefs.GetInt("Quality", 2);
        ChangeQuality();
        fpsSlider.value = PlayerPrefs.GetInt("FPS", 10);
        ChangeFPS();

        resolutionsDropdown.ClearOptions();
 
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

        string resolution = PlayerPrefs.GetString("Resolution", "");
        if (resolution == "")
            return;
        ChangeResolution(resolution);
        int index = resolutionsDropdown.options.FindIndex((i) => { return i.text.Equals(resolution); });
        resolutionsDropdown.value = index;
    }

    public void ChangeResolution(string resolution = "") { 
        string fullRez;
        if(resolution == "")
            fullRez = resolutionsList[resolutionsDropdown.value].ToString();
        else
            fullRez = resolution;

        string width = fullRez.Substring(0, fullRez.IndexOf(' '));
        string height = fullRez.Substring(fullRez.IndexOf('x') + 2, fullRez.Length - (fullRez.IndexOf('x') + 2));

        Screen.SetResolution(System.Int32.Parse(width), System.Int32.Parse(height), Screen.fullScreen);
        PlayerPrefs.SetString("Resolution", fullRez);
    }

    public void ToggleFullscreen()
    {
        Screen.fullScreen = fullscreenToggle.isOn;
        PlayerPrefs.SetInt("Fullscreen", fullscreenToggle.isOn ? 1 : 0);
    }

    public void ChangeQuality()
    {
        int qualityIndex = (int)qualitySlider.value;
        PlayerPrefs.SetInt("Quality", qualityIndex);

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
    }

    public void ChangeFPS()
    {
        int FPS = (int)fpsSlider.value;
        PlayerPrefs.SetInt("FPS", FPS);

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
        PlayerPrefs.SetFloat("Volume", volume);
    }
}
