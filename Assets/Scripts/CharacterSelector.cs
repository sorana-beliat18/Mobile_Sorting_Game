using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class CharacterSelector : MonoBehaviour
{
    [System.Serializable]
    public class OutfitData
    {
        public Sprite outfitSprite;
        [Range(0.5f, 2.0f)] public float customScale = 1f;
        [Range(-200f, 200f)] public float verticalShiftY = 0f;
    }

    [System.Serializable]
    public class CharacterData
    {
        public string name;
        public Image characterObj;
        public Sprite undressedSprite;
        [Range(0.5f, 2.0f)] public float undressedScale = 1f;
        [HideInInspector] public Vector2 initialAnchoredPosition;
        public OutfitData[] outfits;
    }

    [Header("Configurare Personaje")]
    public CharacterData[] allCharacters;
    public string nextLevelName = "Game_Map"; 

    private int currentIndex = 0;       
    private int currentOutfitIndex = -1; // -1= without outfit

    void Start()
    {
        SalvarePozitiiInitiale();
        UpdateVisibleCharacter();
        ShowUndressed();
    }

    void SalvarePozitiiInitiale()
    {
        foreach (var charData in allCharacters)
        {
            if (charData.characterObj != null)
            {
                RectTransform rectTrans = charData.characterObj.GetComponent<RectTransform>();
                charData.initialAnchoredPosition = rectTrans.anchoredPosition;
            }
        }
    }

    // --- ARROWS ---
    public void ChangeAnimal(int direction)
    {
        currentIndex += direction;
        if (currentIndex >= allCharacters.Length) currentIndex = 0;
        else if (currentIndex < 0) currentIndex = allCharacters.Length - 1;
        currentOutfitIndex = -1;
        UpdateVisibleCharacter();
        ShowUndressed();
    }

    // --- OUTFIT BUTTONS ---
    public void ChangeOutfit(int outfitIndex)
    {
        CharacterData currentCharacter = allCharacters[currentIndex];

        if (outfitIndex >= 0 && outfitIndex < currentCharacter.outfits.Length)
        {
            currentOutfitIndex = outfitIndex;

            OutfitData selectedOutfit = currentCharacter.outfits[outfitIndex];
            RectTransform rectTrans = currentCharacter.characterObj.GetComponent<RectTransform>();
            currentCharacter.characterObj.sprite = selectedOutfit.outfitSprite;
            float scale = selectedOutfit.customScale;
            currentCharacter.characterObj.transform.localScale = new Vector3(scale, scale, 1f);

            float newY = currentCharacter.initialAnchoredPosition.y + selectedOutfit.verticalShiftY;
            rectTrans.anchoredPosition = new Vector2(currentCharacter.initialAnchoredPosition.x, newY);
        }
    }

    // --- BUTTON DONE---
    public void ConfirmSelectionAndStart()
    {
        // 1.Save data
        PlayerPrefs.SetInt("SelectedCharacterIndex", currentIndex);
        PlayerPrefs.SetInt("SelectedOutfitIndex", currentOutfitIndex);
        PlayerPrefs.Save(); // Salvam fizic pe disk

        // 2. Next scene
        SceneManager.LoadScene(nextLevelName);
    }

    void UpdateVisibleCharacter()
    {
        for (int i = 0; i < allCharacters.Length; i++)
        {
            allCharacters[i].characterObj.gameObject.SetActive(i == currentIndex);
        }
    }

    void ShowUndressed()
    {
        CharacterData currentCharacter = allCharacters[currentIndex];
        RectTransform rectTrans = currentCharacter.characterObj.GetComponent<RectTransform>();

        if (currentCharacter.undressedSprite != null)
        {
            currentCharacter.characterObj.sprite = currentCharacter.undressedSprite;
            float scale = currentCharacter.undressedScale;
            currentCharacter.characterObj.transform.localScale = new Vector3(scale, scale, 1f);
            rectTrans.anchoredPosition = currentCharacter.initialAnchoredPosition;
        }
    }
}