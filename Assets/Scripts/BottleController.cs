using UnityEngine;

public class BottleController : MonoBehaviour
{
    [Header("Configuration")]
    // Trage aici imaginile cu lichid (Liquid_1, Liquid_2...). 
    // Dacă pui 3 imagini, sticla va avea capacitate 3. Dacă pui 4, va avea 4.
    public SpriteRenderer[] liquidSprites;

    // Această listă se va redimensiona automat
    public Color[] availableColors;

    [Header("Live Data")]
    public int[] liquidLayers; // Nu îi mai dăm mărime fixă aici

    void Awake()
    {
        // Inițializăm datele în funcție de câte imagini (Liquid Sprites) ai pus în Inspector
        // Dacă ai pus 3 sprite-uri, sticla va avea 3 locuri logic.
        if (liquidSprites != null)
        {
            liquidLayers = new int[liquidSprites.Length];
        }
    }

    public void SetupBottle(int[] newLayers, Color[] levelColors)
    {
        availableColors = levelColors;

        // Siguranță: Ne asigurăm că array-ul intern are mărimea corectă
        if (liquidLayers == null || liquidLayers.Length != liquidSprites.Length)
        {
            liquidLayers = new int[liquidSprites.Length];
        }

        // Copiem datele (dar nu mai mult decât încap!)
        for (int i = 0; i < liquidLayers.Length; i++)
        {
            if (i < newLayers.Length)
            {
                liquidLayers[i] = newLayers[i];
            }
            else
            {
                liquidLayers[i] = 0; // Umplem restul cu gol
            }
        }

        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        // Mergem doar până la câte sprite-uri există (liquidSprites.Length)
        for (int i = 0; i < liquidSprites.Length; i++)
        {
            // Siguranță extra: Dacă ai uitat să pui un sprite în slot
            if (liquidSprites[i] == null) continue;

            int colorID = liquidLayers[i];

            if (colorID == 0)
            {
                liquidSprites[i].enabled = false;
            }
            else
            {
                liquidSprites[i].enabled = true;
                int colorIndex = colorID - 1;

                if (availableColors != null && colorIndex < availableColors.Length)
                {
                    liquidSprites[i].color = availableColors[colorIndex];
                }
            }
        }
    }

    // --- Funcții ajutătoare dinamice ---

    public int GetTopColorID()
    {
        for (int i = liquidLayers.Length - 1; i >= 0; i--)
        {
            if (liquidLayers[i] != 0) return liquidLayers[i];
        }
        return 0;
    }

    public int GetFreeSpaceCount()
    {
        int freeSpace = 0;
        for (int i = liquidLayers.Length - 1; i >= 0; i--)
        {
            if (liquidLayers[i] == 0) freeSpace++;
            else break;
        }
        return freeSpace;
    }
}