using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordAttackUi : MonoBehaviour
{
    static public WordAttackUi Instance;

    [SerializeField] RectTransform healthBarFill;
    [SerializeField] GameObject gameOverPopup;

    float maxHealthWidth;
    float currentHealth = 1;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        maxHealthWidth = healthBarFill.rect.width;
    }

    public void IncrementHealth(float healthDeltaPercent)
    {
        currentHealth += healthDeltaPercent;
        if (currentHealth <= 0) {
            gameOverPopup.SetActive(true);
            Time.timeScale = 0;
        } else {
            healthBarFill.sizeDelta = new Vector2(currentHealth * maxHealthWidth, healthBarFill.sizeDelta.y);
        }

    }


}
