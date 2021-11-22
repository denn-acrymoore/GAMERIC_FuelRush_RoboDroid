using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject settingsPanel;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private TextMeshProUGUI volumeValueText;

    public void Start()
    {
        mainMenuPanel.SetActive(true);
        creditsPanel.SetActive(false);
        settingsPanel.SetActive(false);

        float initialVolumeValue;
        audioMixer.GetFloat("myMusicVolume", out initialVolumeValue);

        float volumePercentage = initialVolumeValue + 80;
        volumePercentage = Mathf.Round(volumePercentage);
        volumeValueText.SetText(volumePercentage + " %");
    }

    public void OnVolumeChanged(float volume)
    {
        audioMixer.SetFloat("myMusicVolume", volume);

        float volumePercentage = volume + 80;
        volumePercentage = Mathf.Round(volumePercentage);
        volumeValueText.SetText(volumePercentage + " %");
    }

    public void OnClickBackButton()
    {
        mainMenuPanel.SetActive(true);
        creditsPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    public void OnClickPlayButton()
    {
        SceneManager.LoadScene("Level01");
    }

    public void OnClickCreditsButton()
    {
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void OnClickSettingsButton()
    {
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void OnClickExitButton()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
			Application.Quit();
        #endif
    }
}
