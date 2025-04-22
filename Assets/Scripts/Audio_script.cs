using UnityEngine;
using UnityEngine.UI;   // for Slider
using UnityEngine.SceneManagement;

public class BackgroundMusic_Script : MonoBehaviour
{
    public static BackgroundMusic_Script instance;

    [Header("Audio")]
    [Tooltip("The AudioSource component playing your BGM")]
    public AudioSource musicSource;

    private const string VolumePrefKey = "MusicVolume";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            float savedVol = PlayerPrefs.GetFloat(VolumePrefKey, 1f);
            SetVolume(savedVol);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetVolume(float volume)
    {
        if (musicSource != null)
            musicSource.volume = volume;

        PlayerPrefs.SetFloat(VolumePrefKey, volume);
        PlayerPrefs.Save();
    }
}
