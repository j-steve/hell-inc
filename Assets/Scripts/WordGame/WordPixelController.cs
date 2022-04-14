using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WordPixelController : MonoBehaviour
{
    /** Number of seconds to destroy the player word after one of its pixels collides with an enemy pixel. Prevents the map from filling up with player pixels. */
    const float PLAYER_WORD_DESTROY_COUNTDOWN = 1;
    const float ENEMY_WORD_DESTROY_COUNTDOWN = 2;

    public bool isFromPlayer;
    public int pixelMoveForce;

    [SerializeField] ParticleSystem explosion;

    float? wordDestroyTime = null;
    bool isDestroyed = false;
    new Collider collider;
    MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        collider = GetComponent<Collider>();
        meshRenderer = GetComponent<MeshRenderer>();
        //GetComponent<Rigidbody>().AddForce(new Vector3(pixelMoveForce, 1));
    }

    private void Update()
    {
        if (wordDestroyTime.HasValue && Time.time >= wordDestroyTime) {
            Destroy(gameObject.GetComponentInParent<WordController>().gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isFromPlayer) {
            if (collision.gameObject.tag == "EnemyPixel" && !wordDestroyTime.HasValue) {
                wordDestroyTime = Time.time + PLAYER_WORD_DESTROY_COUNTDOWN;
            }
        } else if (!isDestroyed && (collision.relativeVelocity.magnitude > 20 || collision.gameObject.tag == "Goal")) {
            explosion.Play();
            meshRenderer.enabled = false;
            collider.enabled = false;
            isDestroyed = true;
            if (!wordDestroyTime.HasValue) {
                wordDestroyTime = Time.time + ENEMY_WORD_DESTROY_COUNTDOWN;
            }
        }
    }

}
