using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class ShooterController : MonoBehaviour
{
    const float PLAYER_MOVE_SPEED_MIN = 50;
    const float PLAYER_MOVE_SPEED_MAX = 200;
    const float MIN_SHOOT_INTERVAL = 0.25f;
    const float MAX_SHOOT_INTERVAL = MIN_SHOOT_INTERVAL / 10;

    static List<string> PLAYER_RESPONSE_WORDS = new List<string>() {
       "yup.", "totally.", "really?","oh.", "wow.", "woah.", "no way.", "huh.", "uh-huh.", "yikes." 
        // TODO: add support for multiple words, e.g.  "that's crazy.",   "say what?" 
    };

    [SerializeField] GameObject bulletEjectionPoint; 
    [SerializeField] WordSpawnerController wordSpawner;

    float lastShootTime = 0;

    public void Initialize(CombatModifiers combatModifiers)
    {
        // Reset the scene.
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

        float effectiveShootInterval = MIN_SHOOT_INTERVAL;
        effectiveShootInterval += Player.Instance.Modifiers.Envy / PlayerModifiers.MAX_LEVEL * (MAX_SHOOT_INTERVAL - MIN_SHOOT_INTERVAL);
        if (Input.GetButtonDown("Jump") && Time.time - lastShootTime > effectiveShootInterval) {
            wordSpawner.SpawnWord(PLAYER_RESPONSE_WORDS.GetRandom(), bulletEjectionPoint.transform.position);
            lastShootTime = Time.time;
        }
    }

}