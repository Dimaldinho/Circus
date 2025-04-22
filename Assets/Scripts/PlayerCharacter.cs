using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [Header("Which player is this?")]
    [Range(1, 2)]
    public int playerID = 1;

    [Header("Character Data")]
    public CharacterDatabase characterDB;

    [Header("Sprite Renderer to update")]
    public SpriteRenderer artworkSprite;

    private int selectedOption = 0;
    private string prefsKey;

    void Awake()
    {
        // Build a unique PlayerPrefs key for this player
        prefsKey = $"selectedOption_P{playerID}";
    }

    void Start()
    {
        // Load saved choice, or default to 0
        selectedOption = PlayerPrefs.GetInt(prefsKey, 0);
        UpdateCharacter();
    }

    private void UpdateCharacter()
    {
        // Pull from your database and apply the sprite
        Character character = characterDB.GetCharacter(selectedOption);
        artworkSprite.sprite = character.characterSprite;
    }

    /// <summary>
    /// Call this if you ever want to change the character at runtime
    /// (e.g. if you have “Next/Back” buttons in‑game).
    /// </summary>
    public void SetSelectedOption(int optionIndex)
    {
        selectedOption = Mathf.Clamp(optionIndex, 0, characterDB.CharacterCount - 1);
        PlayerPrefs.SetInt(prefsKey, selectedOption);
        PlayerPrefs.Save();
        UpdateCharacter();
    }
}
