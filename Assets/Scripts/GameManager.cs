using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("Level Configuration")]
    public Color[] levelColorPalette;   
    public BottleController[] bottles; 

    private BottleController selectedBottle;
    private bool isLevelComplete = false;

    void Start()
    {
        GenerateLevel1();
    }

    void GenerateLevel1()
    {
        List<int> liquidDeck = new List<int>();
        for (int i = 0; i < 3; i++) liquidDeck.Add(1);
        for (int i = 0; i < 3; i++) liquidDeck.Add(2);
        for (int i = 0; i < 3; i++) liquidDeck.Add(3);
        bool isDeckValid = false;
        while (!isDeckValid)
        {
            for (int i = 0; i < liquidDeck.Count; i++)
            {
                int temp = liquidDeck[i];
                int randomIndex = Random.Range(i, liquidDeck.Count);
                liquidDeck[i] = liquidDeck[randomIndex];
                liquidDeck[randomIndex] = temp;
            }
            isDeckValid = true; 
            for (int b = 0; b < bottles.Length - 1; b++)
            {
                int baseNote = liquidDeck[b * 3];
                int middleNote = liquidDeck[b * 3 + 1];
                int topNote = liquidDeck[b * 3 + 2];
                if (baseNote == middleNote && middleNote == topNote)
                {
                    isDeckValid = false; 
                    break;
                }
            }
        }
        int cardIndex = 0;
        for (int b = 0; b < bottles.Length; b++)
        {
            int[] startingLayers = new int[3];
            if (b < bottles.Length - 1)
            {
                for (int layer = 0; layer < 3; layer++)
                {
                    startingLayers[layer] = liquidDeck[cardIndex];
                    cardIndex++;
                }
            }
            else
            {
                for (int layer = 0; layer < 3; layer++) startingLayers[layer] = 0;
            }

            bottles[b].SetupBottle(startingLayers, levelColorPalette);
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
        GenerateLevel1();

        Debug.Log("Level Restarted!");
    }

    public void GoToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void ChangeGlobalVolume(float volumeLevel)
    {
        AudioListener.volume = volumeLevel;
    }
    void CheckWinCondition()
    {
        foreach (BottleController bottle in bottles)
        {
            int topColor = bottle.GetTopColorID();
            if (topColor == 0) continue; 

            if (bottle.liquidLayers[0] != topColor ||
                bottle.liquidLayers[1] != topColor ||
                bottle.liquidLayers[2] != topColor)
            {
                return; // There are still mixed bottles, the game continues
            }
        }
        isLevelComplete = true;
        Debug.Log("Level complete!");
        TriggerLevelCompleteUI();
    }
    void TriggerLevelCompleteUI()
    {
        Debug.Log("Level Complete Scene");
    }
}