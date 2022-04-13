using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordAttackUi : MonoBehaviour
{ 
    [SerializeField] RectTransform healthBarFill;

    float maxHealthWidth; 

    // Start is called before the first frame update
    void Start()
    { 
        maxHealthWidth = healthBarFill.rect.width;
    }

    public void UpdateHealthBar(float currentHealth)
    {
        healthBarFill.sizeDelta = new Vector2(currentHealth * maxHealthWidth, healthBarFill.sizeDelta.y);
    }

}
