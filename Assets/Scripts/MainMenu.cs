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
    }

    public void PlayRandomGame()
    {
        //SceneManager.LoadScene("");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
