using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("Level Settings")]
    public int numberOfColors = 3;

    // AICI E SCHIMBAREA: Capacitatea sticlei
    public int bottleCapacity = 3;

    [Header("References")]
    public Color[] levelColorPalette;
    public BottleController[] bottles;

    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        // ... (Validari) ...

        List<int> liquidDeck = new List<int>();

        // Calculăm câte segmente punem. 
        // Dacă sticla e de 3 și avem 3 culori, să zicem că punem 2 segmente per culoare ca să fie rezolvabil.
        // Sau poți lăsa segmentsPerColor configurabil. 
        // Pentru simplitate acum: umplem tot pachetul necesar.

        // Câte bucăți de fiecare culoare? (Sticle - 1) este o regulă bună pt solvabilitate, 
        // dar hai să folosim bottleCapacity ca referință.
        int segmentsPerColor = bottleCapacity - 1; // Lăsăm un loc liber per culoare "ideală"

        for (int i = 1; i <= numberOfColors; i++)
        {
            for (int j = 0; j < segmentsPerColor; j++)
            {
                liquidDeck.Add(i);
            }
        }

        // Shuffle
        for (int i = 0; i < liquidDeck.Count; i++)
        {
            int temp = liquidDeck[i];
            int randomIndex = Random.Range(i, liquidDeck.Count);
            liquidDeck[i] = liquidDeck[randomIndex];
            liquidDeck[randomIndex] = temp;
        }

        // Împărțim
        int cardIndex = 0;
        foreach (BottleController bottle in bottles)
        {
            // Folosim capacitatea setată în GameManager
            int[] newLayers = new int[bottleCapacity];

            for (int layer = 0; layer < bottleCapacity; layer++)
            {
                if (cardIndex < liquidDeck.Count)
                {
                    newLayers[layer] = liquidDeck[cardIndex];
                    cardIndex++;
                }
                else
                {
                    newLayers[layer] = 0;
                }
            }
            bottle.SetupBottle(newLayers, levelColorPalette);
        }
    }
}