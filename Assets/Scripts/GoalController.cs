using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    const float pixelHealthDamage = 0.005f;

    Material material;

    private void Start()
    {
        material = GetComponent<Renderer>().material;
        material.color = Color.blue;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 20) {
            WordAttackUi.Instance.IncrementHealth(-pixelHealthDamage);
            //float impactDamage = (collision.relativeVelocity.magnitude  / 100);
            float newBrightness = material.color.r + pixelHealthDamage;
            Color newColor = new Color(newBrightness, newBrightness, 1);
            material.color = newColor;
        }
    }
}
