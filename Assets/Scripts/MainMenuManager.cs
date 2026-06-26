using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    public void PlayGame()
    {
        int savedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        if (savedLevel == 1)
        {
            SceneManager.LoadScene("CustomizationZone");
        }
        else
        {
            SceneManager.LoadScene("Game_Map");
        }
    }
    public void OpenSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
    }
    public void GoToMap()
    {
        SceneManager.LoadScene("Game_Map");
    }
}