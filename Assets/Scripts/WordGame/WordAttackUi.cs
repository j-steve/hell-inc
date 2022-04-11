using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordAttackUi : MonoBehaviour
{ 

    [SerializeField] RectTransform healthBarFill;
    [SerializeField] GameObject gameOverPopup;

    float maxHealthWidth; 

    // Start is called before the first frame update
    void Start()
    { 
        maxHealthWidth = healthBarFill.rect.width;
        Initialize();
    }
    public void Initialize()
    {
        gameOverPopup.SetActive(false);
    }

    public void UpdateHealthBar(float currentHealth)
    {
        healthBarFill.sizeDelta = new Vector2(currentHealth * maxHealthWidth, healthBarFill.sizeDelta.y);
    }

    public void ShowGameOver()
    {
        gameOverPopup.SetActive(true);
    }


}
