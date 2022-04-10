using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    [SerializeField] GameObject gameOverPopup;
    [SerializeField] ParticleSystem explosion;

    Material material;

    private void Start()
    {
        material = GetComponent<Renderer>().material;
        material.color = Color.blue;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 20) {
            //float impactDamage = (collision.relativeVelocity.magnitude  / 100);
            float newBrightness = material.color.r + 0.01f;
            if (newBrightness >= 1) { 
                gameOverPopup.SetActive(true);
                Time.timeScale = 0;
            } else {
                Color newColor = new Color(newBrightness, newBrightness, 1);
                material.color = newColor;
            }
        }
        Destroy(collision.gameObject);
    }
}
