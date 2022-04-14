using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class ShooterController : MonoBehaviour
{
    const float PLAYER_MOVE_SPEED_MIN = 50;
    const float PLAYER_MOVE_SPEED_MAX = 200;

    static List<string> PLAYER_RESPONSE_WORDS = new List<string>() {
       "yup.", "totally.", "really?","oh.", "wow.", "woah.", "no way.", "huh.", "uh-huh.", "yikes." 
        // TODO: add support for multiple words, e.g.  "that's crazy.",   "say what?" 
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
        float playerMoveSpeed = PLAYER_MOVE_SPEED_MIN;
        playerMoveSpeed += (Player.Instance.Modifiers.Greed / PlayerModifiers.MAX_LEVEL) * (PLAYER_MOVE_SPEED_MAX - PLAYER_MOVE_SPEED_MIN);
        newPosition.y += Input.GetAxis("Vertical") * Time.deltaTime * playerMoveSpeed;
        transform.localPosition = newPosition;
        if (Input.GetButtonDown("Jump")) {
            //BulletController bullet = Instantiate(bulletPrefab, bulletEjectionPoint.transform.position, Quaternion.identity, bulletContainer.transform);
            //bullet.GetComponent<Rigidbody>().AddForce(new Vector3(100, 0, 0), ForceMode.Impulse);
            wordSpawner.SpawnWord(PLAYER_RESPONSE_WORDS.GetRandom(), bulletEjectionPoint.transform.position);
        }
    }

}