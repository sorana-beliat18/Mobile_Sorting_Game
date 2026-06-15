using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel; 
    public void StartNewGame()
    {
        SceneManager.LoadScene("CustomizationZone");
    }
    public void GoToMap()
    {
        SceneManager.LoadScene("Game_Map");
    }
    public void OpenSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
    }
}