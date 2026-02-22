using UnityEngine;

public class BottleController : MonoBehaviour
{
    [Header("Game Configuration")]
    // The palette of colors available in the level (e.g., Pink, Cyan, Yellow)
    public Color[] availableColors;

    [Header("Visual Components")]
    // The 4 sprite renderers representing the liquid layers (from Bottom to Top)
    public SpriteRenderer[] liquidSprites;

    [Header("Current State")]
    // The data representation of the bottle. 
    // 0 = Empty
    // 1 = First Color in availableColors
    // 2 = Second Color, etc.
    public int[] liquidLayers = new int[4];

    void Start()
    {
        // Apply the colors when the game starts
        UpdateVisuals();
    }

    /// <summary>
    /// Updates the visual sprites based on the numeric data in liquidLayers.
    /// </summary>
    public void UpdateVisuals()
    {
        // Loop through all 4 layers
        for (int i = 0; i < liquidLayers.Length; i++)
        {
            int colorID = liquidLayers[i];

            if (colorID == 0)
            {
                // 0 means this layer is empty, so we hide the sprite
                liquidSprites[i].enabled = false;
            }
            else
            {
                // There is liquid here, so show the sprite
                liquidSprites[i].enabled = true;

                // Convert the ID to the actual Color (ID 1 -> Color 0)
                int arrayIndex = colorID - 1;

                // Safety check: Make sure we don't look for a color that doesn't exist
                if (arrayIndex >= 0 && arrayIndex < availableColors.Length)
                {
                    liquidSprites[i].color = availableColors[arrayIndex];
                }
                else
                {
                    Debug.LogError("Color ID " + colorID + " is not defined in 'Available Colors'!");
                }
            }
        }
    }

    // This function detects when the player clicks on the bottle
    void OnMouseDown()
    {
        Debug.Log("Bottle Clicked: " + gameObject.name);

        // TODO: Later we will add the logic to select and pour liquid here
    }
}