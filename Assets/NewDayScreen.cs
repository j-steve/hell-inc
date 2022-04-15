using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewDayScreen : MonoBehaviour
{
    const float SCREEN_DURATION_SECONDS = 3;

    private float? screenEndTime;

    NewDayScreen()
    {
        GameManager.OnDayEnd += ShowNewDayScreen;
    }
    
    private void ShowNewDayScreen()
    {
        gameObject.SetActive(true);
        GetComponentInChildren<TextMeshPro>().text = "Day " + GameManager.WorkDay;
        screenEndTime = Time.time + SCREEN_DURATION_SECONDS;
    }

    private void Update()
    {
        if (screenEndTime.HasValue && screenEndTime.Value >= Time.time) {
            gameObject.SetActive(false);
        }
    }
}
