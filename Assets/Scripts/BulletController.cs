using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private const float LIFETIME_SECONDS = 5;

    private float expirationTime;
    private Material material;
    private float opacity = 1;

    // Start is called before the first frame update
    void Start()
    {
        expirationTime = Time.time + LIFETIME_SECONDS;
        material = gameObject.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > expirationTime) {
            opacity -= Time.deltaTime / 2;
            if (opacity <= 0.8) {
                Destroy(gameObject);
            } else {
                transform.localScale *= opacity;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        expirationTime = Mathf.Min(expirationTime, Time.time + 1.5f);
    }

}
