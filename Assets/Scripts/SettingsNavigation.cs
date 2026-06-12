using UnityEngine;

public class SettingsNavigation : MonoBehaviour
{
    public void RetryCurrentLevel()
    {
        int currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentSceneIndex);
    }

    public void ResumeGame()
    {
        gameObject.SetActive(false);
    }
}
