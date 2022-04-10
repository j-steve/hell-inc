using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public float MasterVolume;
    public float SoundEffectVolume;
    public float MouseSensitivy;
    public int MouseInverted;

    // Start is called before the first frame update
    void Start()
    {
        MasterVolume = Utilities.GetMasterVolume();
        SoundEffectVolume = Utilities.GetSoundEffectVolume();
        MouseSensitivy = Utilities.GetMouseSensitivity();
        MouseInverted = Utilities.GetMouseInversion();
        GetComponentsInChildren<UnityEngine.UI.Slider>()[0].value = MasterVolume;
        GetComponentsInChildren<UnityEngine.UI.Slider>()[1].value = SoundEffectVolume;
        GetComponentsInChildren<UnityEngine.UI.Slider>()[2].value = MouseSensitivy;
    }
    public void Save()
    {
        Utilities.SetSettings(new Settings(MasterVolume, SoundEffectVolume, MouseSensitivy, MouseInverted));
    }
    public void MainVolumeControl(System.Single vol)
    {
        gameObject.GetComponentsInParent<AudioSource>()[0].volume = vol;
        MasterVolume = vol;
    }

    public void SoundEffectControl(System.Single vol)
    {
        gameObject.GetComponentsInParent<AudioSource>()[1].volume = vol;
        SoundEffectVolume = vol;
    }
    public void MouseSensitivityControl(System.Single vol)
    {
        Debug.Log("vol is: " + vol);
    }
    public void MouseInvertedControl(System.Boolean vol)
    {
        Debug.Log("vol is: " + vol);
    }
}
