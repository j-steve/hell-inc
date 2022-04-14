using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class ShooterController : MonoBehaviour
{
    static List<string> PLAYER_RESPONSE_WORDS = new List<string>() {
       "yup.", "totally.", "really?", "that's crazy.", "oh.", "wow.", "woah.", "no way.", "huh.", "uh-huh.", "say what?", "yikes."
    };

    [SerializeField] GameObject bulletEjectionPoint;
    [SerializeField] GameObject bulletContainer;
    [SerializeField] WordSpawnerController wordSpawner;
    [SerializeField] BulletController bulletPrefab;
    [SerializeField] float shooterMoveSpeed = 50;

    public void Initialize(CombatModifiers combatModifiers)
    {
        // Reset the scene.
        foreach(BulletController bullet in bulletContainer.GetComponentsInChildren<BulletController>()) {
            Destroy(bullet.gameObject);
        }
        wordSpawner.Initialize("", combatModifiers);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = transform.localPosition;
        newPosition.y += Input.GetAxis("Vertical") * Time.deltaTime * shooterMoveSpeed;
        transform.localPosition = newPosition;
        if (Input.GetButtonDown("Jump")) {
            //BulletController bullet = Instantiate(bulletPrefab, bulletEjectionPoint.transform.position, Quaternion.identity, bulletContainer.transform);
            //bullet.GetComponent<Rigidbody>().AddForce(new Vector3(100, 0, 0), ForceMode.Impulse);
            wordSpawner.SpawnWord(PLAYER_RESPONSE_WORDS.GetRandom(), bulletEjectionPoint.transform.position);
        }
    }

}