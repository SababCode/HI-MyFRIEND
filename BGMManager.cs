using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class BGMManager : MonoBehaviour
{
    public AudioMixer masterMixer;
    public static float BGMvloume = 1;
    public static float SFXvloume = 1;

    private void Start()
    {
        switch(name)
        {
            case "BGMSlider":
                this.transform.GetComponent<Slider>().value = BGMvloume;
                break;
            case "SFXSlider":
                this.transform.GetComponent<Slider>().value = SFXvloume;
                break;
        }
    }
    public void SetBgm(float sliderVal)
    {
        masterMixer.SetFloat("BGM", Mathf.Log10(sliderVal) * 20);
        BGMvloume = sliderVal;
    }
    public void SetSFX(float sliderVal)
    {
        masterMixer.SetFloat("SFX", Mathf.Log10(sliderVal) * 20);
        SFXvloume = sliderVal;
    }
}
