using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic; // Necesar pentru liste

public class CharacterSelector : MonoBehaviour
{
    [Header("Setari Generale")]
    public bool debugging = false;

    // --- RETETA PENTRU O HAINA ---
    [System.Serializable]
    public class OutfitData
    {
        public Sprite outfitSprite;     // Poza hainei
        [Range(0.5f, 2.0f)]
        public float customScale = 1f;  // SCARA: Cat de mare e

        [Range(-200f, 200f)]
        public float verticalShiftY = 0f; // POZITIA: Cat de sus/jos o mutam (NOU!)
    }

    // --- RETETA PENTRU UN PERSONAJ ---
    [System.Serializable]
    public class CharacterData
    {
        public string name;
        public Image characterObj;      // Obiectul din scena
        public Sprite undressedSprite;
        [Range(0.5f, 2.0f)]
        public float undressedScale = 1f; // Scara pentru dezbracat

        // Aici tinem minte pozitia originala (ascuns in Inspector)
        [HideInInspector] public Vector2 initialAnchoredPosition;

        public OutfitData[] outfits;    // Lista de haine
    }

    [Header("Configurare Personaje")]
    public CharacterData[] allCharacters;

    private int currentIndex = 0;

    void Start()
    {
        // 1. Salvam pozitiile initiale ale tuturor obiectelor
        SalvarePozitiiInitiale();

        // 2. Initializam starea vizuala
        UpdateVisibleCharacter();
        ShowUndressed();
    }

    // Functie noua care memoreaza unde ai pus tu obiectele in scena
    void SalvarePozitiiInitiale()
    {
        foreach (var charData in allCharacters)
        {
            if (charData.characterObj != null)
            {
                // Luam componenta RectTransform pentru a lucra cu pozitii UI
                RectTransform rectTrans = charData.characterObj.GetComponent<RectTransform>();
                charData.initialAnchoredPosition = rectTrans.anchoredPosition;
            }
        }
    }

    // --- SAGETI ---
    public void ChangeAnimal(int direction)
    {
        currentIndex += direction;
        if (currentIndex >= allCharacters.Length) currentIndex = 0;
        else if (currentIndex < 0) currentIndex = allCharacters.Length - 1;

        UpdateVisibleCharacter();
        ShowUndressed();
    }

    // --- BUTOANE OUTFIT ---
    public void ChangeOutfit(int outfitIndex)
    {
        CharacterData currentCharacter = allCharacters[currentIndex];

        if (outfitIndex >= 0 && outfitIndex < currentCharacter.outfits.Length)
        {
            OutfitData selectedOutfit = currentCharacter.outfits[outfitIndex];
            RectTransform rectTrans = currentCharacter.characterObj.GetComponent<RectTransform>();

            // 1. Schimbam poza
            currentCharacter.characterObj.sprite = selectedOutfit.outfitSprite;

            // 2. Aplicam Scara
            float scale = selectedOutfit.customScale;
            currentCharacter.characterObj.transform.localScale = new Vector3(scale, scale, 1f);

            // 3. Aplicam Pozitia Verticala (NOU!)
            // Pozitia finala = Pozitia de baza + Ajustarea ta
            float newY = currentCharacter.initialAnchoredPosition.y + selectedOutfit.verticalShiftY;
            rectTrans.anchoredPosition = new Vector2(currentCharacter.initialAnchoredPosition.x, newY);
        }
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
            // 1. Poza si Scara
            currentCharacter.characterObj.sprite = currentCharacter.undressedSprite;
            float scale = currentCharacter.undressedScale;
            currentCharacter.characterObj.transform.localScale = new Vector3(scale, scale, 1f);

            // 2. Resetam pozitia la cea originala (fara shift) cand e dezbracat
            rectTrans.anchoredPosition = currentCharacter.initialAnchoredPosition;
        }
    }
}