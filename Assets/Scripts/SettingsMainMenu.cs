using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsMainMenu : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Image musicOnImage;
    [SerializeField] private Image musicOffImage;
    [SerializeField] private AudioSource[] sfxSources;

    private const string VolumeKey = "VolumeVolumeLevel";
    private const string MusicToggleKey = "MusicToggleState";

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 1f);
        bool isMusicOn = PlayerPrefs.GetInt(MusicToggleKey, 1) == 1;


        UpdateMusicButtonsVisuals(isMusicOn);
        ApplySFXVolume(savedVolume);

        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }
    public void SetVolume(float value)
    {
        ApplySFXVolume(value);
        PlayerPrefs.SetFloat(VolumeKey, value);
    }
    private void ApplySFXVolume(float vol)
    {
        if (sfxSources != null)
        {
            foreach (AudioSource sfx in sfxSources)
            {
                if (sfx != null) sfx.volume = vol;
            }
        }
    }
    public void TurnMusicOn()
    {
        PlayerPrefs.SetInt(MusicToggleKey, 1);
        if (AudioManager.instance != null) AudioManager.instance.ToggleMusic(true);
        UpdateMusicButtonsVisuals(true);
    }

    public void TurnMusicOff()
    {
        PlayerPrefs.SetInt(MusicToggleKey, 0);
        if (AudioManager.instance != null) AudioManager.instance.ToggleMusic(false);
        UpdateMusicButtonsVisuals(false);
    }
    private void UpdateMusicButtonsVisuals(bool isMusicOn)
    {
        if (musicOnImage != null)
        {
            Color colorOn = musicOnImage.color;
            colorOn.a = isMusicOn ? 1f : 0.4f;
            musicOnImage.color = colorOn;
        }

        if (musicOffImage != null)
        {
            Color colorOff = musicOffImage.color;
            colorOff.a = !isMusicOn ? 1f : 0.4f;
            musicOffImage.color = colorOff;
        }
    }
    public void ResetGameProgress()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Game progress has been deleted!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CloseSettings()
    {
        gameObject.SetActive(false);
    }

   // public void QuitGame()
    //{
      //  Application.Quit();
        //Debug.Log("Game quit!");
    //}
}