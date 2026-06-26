using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Level Configuration")]
    public int levelNumber = 1;
    public Color[] levelColorPalette;   
    public BottleController[] bottles;
    public GameObject WinScreen;

    private BottleController selectedBottle;
    private bool isLevelComplete = false;


    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        int numColors = levelColorPalette.Length;
        int numBottles = bottles.Length;
        int[][] simulatedBottles = new int[numBottles][];
        for (int i = 0; i < numBottles; i++)
        {
            simulatedBottles[i] = new int[3];
            if (i < numColors)
            {
                simulatedBottles[i][0] = i + 1; 
                simulatedBottles[i][1] = i + 1;
                simulatedBottles[i][2] = i + 1;
            }
        }
        // Reverse Shuffle
        int moves = 0;
        int attempts = 0;
        int maxMoves = numColors * 20;

        while (moves < maxMoves && attempts < 1000)
        {
            attempts++;
            int takeIdx = Random.Range(0, numBottles);
            int putIdx = Random.Range(0, numBottles);
            if (takeIdx == putIdx) continue;

            int takeCount = 0;
            int topColor = 0;
            for (int layer = 0; layer < 3; layer++)
            {
                if (simulatedBottles[takeIdx][layer] != 0)
                {
                    takeCount = layer + 1;
                    topColor = simulatedBottles[takeIdx][layer];
                }
            }

            if (takeCount == 0) continue;

            bool canTake = true;
            if (takeCount > 1)
            {
                int colorBelow = simulatedBottles[takeIdx][takeCount - 2];
                if (colorBelow != topColor) canTake = false;
            }

            if (!canTake) continue;

            int putCount = 0;
            for (int layer = 0; layer < 3; layer++)
            {
                if (simulatedBottles[putIdx][layer] != 0) putCount = layer + 1;
            }

            if (putCount == 3) continue;
            simulatedBottles[takeIdx][takeCount - 1] = 0;
            simulatedBottles[putIdx][putCount] = topColor;

            moves++;
            attempts = 0;
        }
        for (int b = 0; b < bottles.Length; b++)
        {
            bottles[b].SetupBottle(simulatedBottles[b], levelColorPalette);
        }
    }

    public void HandleBottleClick(BottleController clickedBottle)
    {
        if (isLevelComplete) return;

        if (selectedBottle == null)
        {
            if (clickedBottle.GetTopColorID() != 0)
            {
                selectedBottle = clickedBottle;
                selectedBottle.SetSelected(true);
                Debug.Log("Bottle selected: " + clickedBottle.gameObject.name);
            }
        }
        else if (selectedBottle == clickedBottle)
        {
            selectedBottle.SetSelected(false);
            selectedBottle = null;
            Debug.Log("Deselected.");
        }
        else
        {
            int sourceColor = selectedBottle.GetTopColorID();
            int targetColor = clickedBottle.GetTopColorID();
            int spaceInTarget = clickedBottle.GetFreeSpaceCount();

            // Move allowed
            if (spaceInTarget > 0 && (targetColor == 0 || targetColor == sourceColor))
            {
                int amountInSource = selectedBottle.GetTopColorCount();
                int amountToMove = Mathf.Min(amountInSource, spaceInTarget);

                selectedBottle.SetSelected(false);

                selectedBottle.ExtractLiquid(amountToMove);
                clickedBottle.AddLiquid(sourceColor, amountToMove);
                GameObject audioMgr = GameObject.Find("AudioManager");
                if (audioMgr != null)
                {
                    AudioSource[] sources = audioMgr.GetComponents<AudioSource>();
                    if (sources.Length > 0) sources[0].Play(); 
                }

                Debug.Log("Liquid moved successfully!");

                selectedBottle = null;
                CheckWinCondition();
            }
            else
            {
                // Move not allowed
                selectedBottle.SetSelected(false);
                selectedBottle = null;
                Debug.Log("Invalid move!");
                GameObject audioMgr = GameObject.Find("AudioManager");
                if (audioMgr != null)
                {
                    AudioSource[] sources = audioMgr.GetComponents<AudioSource>();
                    if (sources.Length > 1) sources[1].Play();
                }
            }
        }
    }
    public void RetryLevel()
    {
        isLevelComplete = false;
        if (selectedBottle != null)
        {
            selectedBottle.SetSelected(false);
            selectedBottle = null;
        }
        GenerateLevel();
        Debug.Log("Level Restarted!");
    }

    public void GoToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main_Menu");
    }

    public void ChangeGlobalVolume(float volumeLevel)
    {
        AudioListener.volume = volumeLevel;
    }
    void CheckWinCondition()
    {
        foreach (BottleController bottle in bottles)
        {
            if (bottle != null)
            {
                bottle.CheckAndForceRevealMystery();
            }
        }

        foreach (BottleController bottle in bottles)
        {
            int topColor = bottle.GetTopColorID();
            if (topColor == 0) continue; 

            if (bottle.liquidLayers[0] != topColor ||
                bottle.liquidLayers[1] != topColor ||
                bottle.liquidLayers[2] != topColor)
            {
                return; 
            }
        }
        isLevelComplete = true;
        Debug.Log("Level complete!");
        Invoke("TriggerLevelCompleteUI", 0.6f);
    }
    void TriggerLevelCompleteUI()
    {
        Debug.Log("Level Complete Scene");
        WinScreen.SetActive(true);
        int nextLevel = levelNumber + 1; 
        if (nextLevel > PlayerPrefs.GetInt("UnlockedLevel", 1))
        {
            PlayerPrefs.SetInt("UnlockedLevel", nextLevel); 
            PlayerPrefs.Save();
            Debug.Log("Progres salvat! Noul nivel deblocat este: " + nextLevel);
        }
    }

}