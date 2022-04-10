using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WordPixelController : MonoBehaviour
{
    [SerializeField] ParticleSystem explosion;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(new Vector3(-1000, 1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 20) {
            ParticleSystem pixelExplosion = Instantiate(explosion);
            explosion.transform.position = collision.gameObject.transform.position;
            Destroy(gameObject);
        }
    }

}
