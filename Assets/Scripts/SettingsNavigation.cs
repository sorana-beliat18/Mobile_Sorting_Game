using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;

public class SettingsNavigation : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private AudioSource[] sfxSources;
    private const string VolumeKey = "VolumeVolumeLevel";
    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 1f);
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
                if (sfx != null)
                {
                    sfx.volume = vol;
                }
            }
        }
    }
    public void RetryCurrentLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void ResumeGame()
    {
        gameObject.SetActive(false);
    }
}
