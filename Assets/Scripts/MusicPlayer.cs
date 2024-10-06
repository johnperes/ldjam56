using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance;
    
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }
}

