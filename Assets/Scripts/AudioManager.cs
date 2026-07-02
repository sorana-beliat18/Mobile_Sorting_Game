using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioSource musicSource;
    private const string MusicToggleKey = "MusicToggleState";
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
            return;
        }
        LoadMusicSettings();
    }
    private void LoadMusicSettings()
    {
        if (musicSource != null)
        {
            //1=On, 0=Off
            bool isMusicOn = PlayerPrefs.GetInt(MusicToggleKey, 1) == 1;
            musicSource.mute = !isMusicOn;
        }
    }
    public void ToggleMusic(bool isMusicOn)
    {
        PlayerPrefs.SetInt(MusicToggleKey, isMusicOn ? 1 : 0);
        if (musicSource != null)
        {
            musicSource.mute = !isMusicOn;
        }
    }
}