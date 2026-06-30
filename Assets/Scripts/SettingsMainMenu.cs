using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsMainMenu : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource[] sfxSources;

    private const string VolumeKey = "VolumeVolumeLevel";
    private const string MusicKey = "MusicVolumeLevel";

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 1f);
        float savedMusic = PlayerPrefs.GetFloat(MusicKey, 1f);
       
        if (musicSource != null)
        {
            musicSource.volume = savedMusic;
        }

        if (sfxSources != null)
        {
            foreach (AudioSource sfx in sfxSources)
            {
                if (sfx != null) sfx.volume = savedVolume;
            }
        }

        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        if (musicSlider != null)
        {
            musicSlider.value = savedMusic;
            musicSlider.onValueChanged.AddListener(SetMusic);
        }
    }

    public void SetVolume(float value)
    {
        if (sfxSources != null)
        {
            foreach (AudioSource sfx in sfxSources)
            {
                if (sfx != null) sfx.volume = value;
            }
        }
        Debug.Log("SFX Volume set to: " + value);
        PlayerPrefs.SetFloat(VolumeKey, value);
    }

    public void SetMusic(float value)
    {
        if (musicSource != null)
        {
            musicSource.volume = value;
        }
        Debug.Log("Music set to: " + value);
        PlayerPrefs.SetFloat(MusicKey, value);
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