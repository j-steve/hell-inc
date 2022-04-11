using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WordPixelController : MonoBehaviour
{
    [SerializeField] ParticleSystem explosion;

    bool isDestroyed = false;
    new Collider collider;
    MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        collider = GetComponent<Collider>();
        meshRenderer = GetComponent<MeshRenderer>();
        GetComponent<Rigidbody>().AddForce(new Vector3(-1000, 1));
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isDestroyed && (collision.relativeVelocity.magnitude > 20 || collision.gameObject.tag == "Goal")) {
            explosion.Play();
            meshRenderer.enabled = false;
            collider.enabled = false;
            isDestroyed = true;
        }
    }

}
