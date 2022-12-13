using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MusicPlay : MonoBehaviour
{
    public SoundManager soundManager;
    private void Awake()
    {
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }
    void Start()
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                soundManager.BGMSoundPlay(0);
                break;
            case 3:
                soundManager.BGMSoundPlay(3);
                break;
            case 4:
                soundManager.BGMSoundPlay(4);
                break;
            case 5:
                soundManager.BGMSoundPlay(5);
                break;
        }
    }
}
