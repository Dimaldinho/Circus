using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterManager : MonoBehaviour
{
    [Header("Player ID (1 or 2)")]
    [Tooltip("Unique ID for this selection panel. Determines which PlayerPrefs key is used.")]
    [Range(1, 2)] 
    public int playerID = 1;

    [Header("Character Data and UI")]
    public CharacterDatabase characterDB;
    public Text nameText;
    public SpriteRenderer artworkSprite;
    public Animator      artworkAnimator;

    private int selectedOption = 0;
    private string prefsKey;

    void Awake()
    {
        // Build a unique prefs key per player
        prefsKey = $"selectedOption_P{playerID}";
    }

    void Start()
    {
        // Load or default
        if (PlayerPrefs.HasKey(prefsKey))
            selectedOption = PlayerPrefs.GetInt(prefsKey);
        else
            selectedOption = 0;

        UpdateCharacterUI();
    }

    public void NextOption()
    {
        selectedOption = (selectedOption + 1) % characterDB.CharacterCount;
        ApplySelection();
    }

    public void BackOption()
    {
        selectedOption = (selectedOption - 1 + characterDB.CharacterCount) % characterDB.CharacterCount;
        ApplySelection();
    }

    private void ApplySelection()
    {
        UpdateCharacterUI();
        PlayerPrefs.SetInt(prefsKey, selectedOption);
        PlayerPrefs.Save();
    }

    private void UpdateCharacterUI()
    {
        Character c = characterDB.GetCharacter(selectedOption);
        artworkSprite.sprite = c.characterSprite;
        nameText.text         = c.characterName;

        if (artworkAnimator != null && c.animatorController != null)
        {
            // swap in the character's controller
            artworkAnimator.runtimeAnimatorController = c.animatorController;
            // restart the default state (layer 0) from the beginning
            var state = artworkAnimator.GetCurrentAnimatorStateInfo(0);
            artworkAnimator.Play(state.fullPathHash, 0, 0f);
        }
    }

    public void ChangeScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }
}
