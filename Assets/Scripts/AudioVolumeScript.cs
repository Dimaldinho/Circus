using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeSlider : MonoBehaviour
{
    public Slider slider;  

    private void Start()
    {
        
        float saved = PlayerPrefs.GetFloat("MusicVolume", 1f);
        slider.value = saved;

       
        slider.onValueChanged.AddListener( BackgroundMusic_Script.instance.SetVolume );
    }
}
