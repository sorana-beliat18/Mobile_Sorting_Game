using UnityEngine;

public class BottleController : MonoBehaviour
{
    [Header("Visual Configuration")]
    public SpriteRenderer maskRenderer;

    [Header("Real-Time Data")]
    // Array of size 3: index 0 = Base, index 1 = Middle, index 2 = Top
    // 0 = Empty, 1 = Color A, 2 = Color B, 3 = Color C
    public int[] liquidLayers = new int[3];

    private Color[] availableColors;
    private Material uniqueMaterial;

    void Awake()
    {
        // We create a unique copy of the material for each individual bottle.
        // If we don't do this, when we color one bottle, they will all color the same!
        if (maskRenderer != null)
        {
            uniqueMaterial = maskRenderer.material;
        }
    }
    public void SetupBottle(int[] newLayers, Color[] levelColors)
    {
        availableColors = levelColors;

        for (int i = 0; i < 3; i++)
        {
            liquidLayers[i] = newLayers[i];
        }

        UpdateVisuals();
    }
    public void UpdateVisuals()
    {
        if (uniqueMaterial == null) return;

        // We convert the numeric IDs into actual colors from the palette
        Color baseColor = GetColorFromID(liquidLayers[0]);
        Color middleColor = GetColorFromID(liquidLayers[1]);
        Color topColor = GetColorFromID(liquidLayers[2]);

        // Send the colors to the shader using the new English references
        uniqueMaterial.SetColor("_Color_1", baseColor);
        uniqueMaterial.SetColor("_Color_2", middleColor);
        uniqueMaterial.SetColor("_Color_3", topColor);

        // We calculate the shader height based on how many layers are not 0 (empty)
        int filledLayers = 0;
        for (int i = 0; i < 3; i++)
        {
            if (liquidLayers[i] != 0) filledLayers++;
        }

        // We map the number of layers to the shader slider (-0.5 is empty, 0.5 is full)
        float fillValue = -0.5f; // Default empty
        if (filledLayers == 1) fillValue = -0.16f; // Adjust these numbers if the lines don't fit perfectly on your model
        if (filledLayers == 2) fillValue = 0.16f;
        if (filledLayers == 3) fillValue = 0.5f;

        uniqueMaterial.SetFloat("_Fill_Amount", fillValue);
    }

    private Color GetColorFromID(int id)
    {
        if (id <= 0 || availableColors == null || id > availableColors.Length)
        {
            Color backgroundColor;
            ColorUtility.TryParseHtmlString("#C8B9A7", out backgroundColor);
            return backgroundColor;
        }

        return availableColors[id - 1];
    }

    // --- LOGIC FUNCTIONS FOR GAME MECHANICS ---

    public int GetTopColorID()
    {
        for (int i = 2; i >= 0; i--)
        {
            if (liquidLayers[i] != 0) return liquidLayers[i];
        }
        return 0; // The bottle is empty
    }

    public int GetFreeSpaceCount()
    {
        int freeSpace = 0;
        for (int i = 2; i >= 0; i--)
        {
            if (liquidLayers[i] == 0) freeSpace++;
            else break;
        }
        return freeSpace;
    }

    public int GetTopColorCount()
    {
        int color = GetTopColorID();
        if (color == 0) return 0;

        int count = 0;
        for (int i = 2; i >= 0; i--)
        {
            if (liquidLayers[i] == color) count++;
            else if (liquidLayers[i] != 0) break;
        }
        return count;
    }

    public void ExtractLiquid(int amount)
    {
        int removed = 0;
        for (int i = 2; i >= 0; i--)
        {
            if (liquidLayers[i] != 0)
            {
                liquidLayers[i] = 0;
                removed++;
                if (removed >= amount) break;
            }
        }
        UpdateVisuals();
    }

    public void AddLiquid(int colorID, int amount)
    {
        int added = 0;
        for (int i = 0; i < 3; i++)
        {
            if (liquidLayers[i] == 0)
            {
                liquidLayers[i] = colorID;
                added++;
                if (added >= amount) break;
            }
        }
        UpdateVisuals();
    }

    void OnMouseDown()
    {
        FindObjectOfType<GameManager>().HandleBottleClick(this);
    }
}