using UnityEngine;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    [System.Serializable]
    public class OutfitData
    {
        public Sprite outfitSprite;
    }

    [System.Serializable]
    public class CharacterData
    {
        public Sprite undressedSprite;
        public OutfitData[] outfits;
    }

    public CharacterData[] allCharacters;
    public float bounceSpeed = 6f;     
    public float bounceHeight = 30f;     
    public float squishAmount = 0.15f;   

    private Image characterImage;
    private Vector2 startPos;
    private Vector3 startScale;
    private float animationTimer;

    void Awake()
    {
        characterImage = GetComponent<Image>();
        startPos = characterImage.rectTransform.anchoredPosition;
        startScale = transform.localScale;
    }

    void OnEnable()
    {
        int charIndex = PlayerPrefs.GetInt("SelectedCharacterIndex", 0);
        int outfitIndex = PlayerPrefs.GetInt("SelectedOutfitIndex", -1);

        if (charIndex >= 0 && charIndex < allCharacters.Length)
        {
            CharacterData cd = allCharacters[charIndex];

            if (outfitIndex >= 0 && outfitIndex < cd.outfits.Length)
            {
                characterImage.sprite = cd.outfits[outfitIndex].outfitSprite;
            }
            else
            {
                characterImage.sprite = cd.undressedSprite;
            }
        }
        animationTimer = 0f;
    }

    void Update()
    {
        animationTimer += Time.deltaTime;
        float newY = startPos.y + Mathf.Abs(Mathf.Sin(animationTimer * bounceSpeed)) * bounceHeight;
        characterImage.rectTransform.anchoredPosition = new Vector2(startPos.x, newY);
        float scaleX = startScale.x + Mathf.Cos(animationTimer * bounceSpeed * 2f) * squishAmount;
        float scaleY = startScale.y - Mathf.Cos(animationTimer * bounceSpeed * 2f) * squishAmount;
        transform.localScale = new Vector3(scaleX, scaleY, 1f);
    }
}