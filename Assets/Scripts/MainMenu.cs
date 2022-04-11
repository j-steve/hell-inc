using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponentsInParent<AudioSource>()[0].volume = Utilities.GetMasterVolume();
        gameObject.GetComponentsInParent<AudioSource>()[1].volume = Utilities.GetSoundEffectVolume();
    }

    public void PlayDailyGame()
    {
        //SceneManager.LoadScene("");
        gameObject.GetComponentsInParent<AudioSource>()[1].Play();
    }

    public void Settings()
    {
        gameObject.GetComponentsInParent<AudioSource>()[1].Play();
    }

    public void PlayRandomGame()
    {
        //SceneManager.LoadScene("");
        gameObject.GetComponentsInParent<AudioSource>()[1].Play();
    }

    public void Exit()
    {
        Application.Quit();
        gameObject.GetComponentsInParent<AudioSource>()[1].Play();
    }
}
