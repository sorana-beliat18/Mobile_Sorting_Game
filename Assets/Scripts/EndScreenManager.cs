using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenManager : MonoBehaviour
{
    [SerializeField] private AudioSource winAudioSource;
    private const string VolumeKey = "VolumeVolumeLevel";
    void Start()
    {
        if (winAudioSource != null)
        {
            float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 1f);
            winAudioSource.volume = savedVolume;
            winAudioSource.Play();
        }
    }
    public void PlayAgain()
    {
        SceneManager.LoadScene("Level_1");
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main_Menu");
    }
}