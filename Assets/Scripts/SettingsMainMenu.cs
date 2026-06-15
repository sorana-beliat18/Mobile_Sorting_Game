using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsMainMenu : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider musicSlider;

    private const string VolumeKey = "VolumeVolumeLevel";
    private const string MusicKey = "MusicVolumeLevel";

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 1f);
        float savedMusic = PlayerPrefs.GetFloat(MusicKey, 1f);

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
        AudioListener.volume = value;
        Debug.Log("Volume set to: " + value);
        PlayerPrefs.SetFloat(VolumeKey, value);
    }

    public void SetMusic(float value)
    {
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