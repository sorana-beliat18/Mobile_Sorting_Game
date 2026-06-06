using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelMarker : MonoBehaviour
{
    public enum LevelState { Locked, Current, Completed }

    [Header("Level Configuration")]
    public int levelNumber = 1;
    public string sceneToLoad = "LevelScene";

    [Header("UI References")]
    public Image glowImage;
    public TextMeshProUGUI numberText;
    public Button myButton;

    [Header("Glow Settings (Colors)")]
    public Color lockedColor = new Color(1f, 0.9f, 0.5f);
    public Color currentColor = Color.yellow;
    public Color completedColor = new Color(1f, 0.4f, 0.8f);

    [Header("Glow Settings (Speeds)")]
    public float lockedPulseSpeed = 1f;
    public float currentPulseSpeed = 2.5f;
    public float completedPulseSpeed = 5f;

    private LevelState currentState;
    private Vector3 initialGlowScale;

    void Start()
    {
        if (glowImage != null) initialGlowScale = glowImage.transform.localScale;

        UpdateLevelStatus();

        myButton.onClick.AddListener(OnLevelClicked);
        if (numberText != null) numberText.text = levelNumber.ToString();
    }

    void UpdateLevelStatus()
    {
        int reachedLevel = PlayerPrefs.GetInt("ReachedLevel", 1);

        if (levelNumber < reachedLevel)
        {
            // Case 1: Completed
            currentState = LevelState.Completed;
            myButton.interactable = true;

            if (glowImage != null)
            {
                glowImage.gameObject.SetActive(true);
                glowImage.color = completedColor;
            }
        }
        else if (levelNumber == reachedLevel)
        {
            // Case 2: Current
            currentState = LevelState.Current;
            myButton.interactable = true; 

            if (glowImage != null)
            {
                glowImage.gameObject.SetActive(true);
                glowImage.color = currentColor;
            }
        }
        else
        {
            // Case 3: Locked
            currentState = LevelState.Locked;
            myButton.interactable = false; 

            if (glowImage != null)
            {
                glowImage.gameObject.SetActive(true);
                glowImage.color = lockedColor;
            }
        }
    }

    void Update()
    {
        if (glowImage == null || !glowImage.gameObject.activeSelf) return;

        float speed = 1f;
        float maxAlpha = 1f;
        float minAlpha = 0f;

        switch (currentState)
        {
            case LevelState.Locked:
                speed = lockedPulseSpeed;
                maxAlpha = 0.3f; 
                minAlpha = 0.1f;
                break;
            case LevelState.Current:
                speed = currentPulseSpeed;
                maxAlpha = 0.8f;
                minAlpha = 0.4f;
                break;
            case LevelState.Completed:
                speed = completedPulseSpeed;
                maxAlpha = 1.0f; 
                minAlpha = 0.6f;
                break;
        }
        float pulse = 1f + Mathf.Sin(Time.time * speed) * 0.1f;
        glowImage.transform.localScale = initialGlowScale * pulse;
        Color c = glowImage.color;
        float midAlpha = (maxAlpha + minAlpha) / 2f;
        float amplitude = (maxAlpha - minAlpha) / 2f;
        c.a = midAlpha + Mathf.Sin(Time.time * speed) * amplitude;
        glowImage.color = c;
    }

    void OnLevelClicked()
    {
        PlayerPrefs.SetInt("SelectedLevel", levelNumber);
        Debug.Log("Clicked on level: " + levelNumber +". Loading scene: " + sceneToLoad );
        UnityEngine.SceneManagement.SceneManager.LoadScene( sceneToLoad );
    }
}