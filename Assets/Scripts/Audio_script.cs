using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic_Script : MonoBehaviour
{
    // Singleton instance
    public static BackgroundMusic_Script instance;

    private void Awake()
    {
        if (instance == null)
        {
            // First time: keep this object across scenes
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // A second copy was created—destroy it
            Destroy(gameObject);
        }
    }
}
