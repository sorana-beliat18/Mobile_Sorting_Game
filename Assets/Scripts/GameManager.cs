using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("Level Configuration")]
    public Color[] levelColorPalette;   // Put 3 beautiful colors in the Inspector
    public BottleController[] bottles;  // Drag the 4 bottles from the scene here

    private BottleController selectedBottle;

    void Start()
    {
        GenerateLevel1();
    }

    void GenerateLevel1()
    {
        // We have 4 bottles in total. Three will be full (mixed), one will be completely empty.
        List<int> liquidDeck = new List<int>();

        // Add 3 segments of Color 1 (ID: 1)
        for (int i = 0; i < 3; i++) liquidDeck.Add(1);
        // Add 3 segments of Color 2 (ID: 2)
        for (int i = 0; i < 3; i++) liquidDeck.Add(2);
        // Add 3 segments of Color 3 (ID: 3)
        for (int i = 0; i < 3; i++) liquidDeck.Add(3);

        bool isDeckValid = false;

        // We keep shuffling the top, middle, and base notes until no bottle gets 3 identical notes
        while (!isDeckValid)
        {
            // Shuffle the deck
            for (int i = 0; i < liquidDeck.Count; i++)
            {
                int temp = liquidDeck[i];
                int randomIndex = Random.Range(i, liquidDeck.Count);
                liquidDeck[i] = liquidDeck[randomIndex];
                liquidDeck[randomIndex] = temp;
            }

            isDeckValid = true; // Assume it's valid until proven otherwise

            // Check if the 3 notes for any bottle ended up exactly the same
            for (int b = 0; b < bottles.Length - 1; b++)
            {
                int baseNote = liquidDeck[b * 3];
                int middleNote = liquidDeck[b * 3 + 1];
                int topNote = liquidDeck[b * 3 + 2];

                // If base, middle, and top notes are identical, the shuffle is invalid
                if (baseNote == middleNote && middleNote == topNote)
                {
                    isDeckValid = false; // We reject this shuffle and loop again
                    break;
                }
            }
        }

        // Distribute the validated liquid notes into the bottles
        int cardIndex = 0;

        for (int b = 0; b < bottles.Length; b++)
        {
            int[] startingLayers = new int[3];

            // Bottles with index 0, 1, and 2 receive the mixed notes. The last bottle (index 3) remains empty.
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
                // The last bottle is empty (all notes set to 0)
                for (int layer = 0; layer < 3; layer++) startingLayers[layer] = 0;
            }

            bottles[b].SetupBottle(startingLayers, levelColorPalette);
        }
    }

    public void HandleBottleClick(BottleController clickedBottle)
    {
        if (selectedBottle == null)
        {
            // Select the first bottle (cannot be empty)
            if (clickedBottle.GetTopColorID() != 0)
            {
                selectedBottle = clickedBottle;
                Debug.Log("Bottle selected: " + clickedBottle.gameObject.name);
            }
        }
        else if (selectedBottle == clickedBottle)
        {
            // Deselect if you click the same bottle
            selectedBottle = null;
            Debug.Log("Deselected.");
        }
        else
        {
            // Clicked on the second bottle (Target)
            int sourceColor = selectedBottle.GetTopColorID();
            int targetColor = clickedBottle.GetTopColorID();
            int spaceInTarget = clickedBottle.GetFreeSpaceCount();

            // Classic rule: do they have the same top color OR is the target completely empty? AND is there available space?
            if (spaceInTarget > 0 && (targetColor == 0 || targetColor == sourceColor))
            {
                int amountInSource = selectedBottle.GetTopColorCount();
                int amountToMove = Mathf.Min(amountInSource, spaceInTarget);

                // Instant logical move (until we add the visual pipette system)
                selectedBottle.ExtractLiquid(amountToMove);
                clickedBottle.AddLiquid(sourceColor, amountToMove);

                Debug.Log("Liquid moved successfully!");

                selectedBottle = null;
                CheckWinCondition();
            }
            else
            {
                // Invalid move
                selectedBottle = null;
                Debug.Log("Invalid move!");
            }
        }
    }

    void CheckWinCondition()
    {
        foreach (BottleController bottle in bottles)
        {
            int topColor = bottle.GetTopColorID();
            if (topColor == 0) continue; // Empty bottles are fine at the end

            // If it has liquid, we check if all 3 layers are equal to that color
            if (bottle.liquidLayers[0] != topColor ||
                bottle.liquidLayers[1] != topColor ||
                bottle.liquidLayers[2] != topColor)
            {
                return; // There are still mixed bottles, the game continues
            }
        }

        Debug.Log("CONGRATULATIONS! YOU WON THE LEVEL!");
    }
}