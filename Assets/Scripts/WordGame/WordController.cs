using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordController : MonoBehaviour
{
    const float PLAYER_BASE_SIZE = 0.5f;
    const float PLAYER_MAX_SIZE_BOOST = 0.5f;

    const int ENEMY_BASE_SPEED = 3000;
    const int PLAYER_BASE_SPEED = 5000;
    const int PLAYER_MAX_SPEED_BOOST = 2000;
    const int MAX_RANDOM_SPEED_BOOST = 1000;
    const int MAX_DIFFICULTY_SPEED_BOOST = 1000;

    public Color color;
    public float heightOffset = 1;
    public float spinSpeed = 0;
    public float size = 1 ;
    public float speed = 1;
    public float verticalMovement = 0;
    public float verticalAgility = 0;
    public int currentDirection = 1;

    public WordController Initialize(CombatModifiers combatModifiers, bool isPlayerWord)
    {
        color = Random.ColorHSV();
        color.r = 1 - color.r * 0.5f;
        color.g = 1 - color.g * 0.5f;
        color.b = 1 - color.b * 0.5f; 
        if (isPlayerWord) {
            size = PLAYER_BASE_SIZE + (GameManager.Player.Modifiers.Gluttony / PlayerModifiers.MAX_LEVEL) * PLAYER_MAX_SIZE_BOOST;
            speed = PLAYER_BASE_SPEED;
            speed += (GameManager.Player.Modifiers.Wrath / PlayerModifiers.MAX_LEVEL) * PLAYER_MAX_SPEED_BOOST;
        } else {  
            heightOffset += (Random.value * 2 - 1) * 40 * combatModifiers.MiniGameSize;
            transform.position = transform.position + new Vector3(0, heightOffset, 0);
            size = 1 * (Random.value + 1) * combatModifiers.ConversationTextSize;
            speed = ENEMY_BASE_SPEED;
            speed += Random.value * MAX_RANDOM_SPEED_BOOST;
            speed += combatModifiers.ConversationTextSpeed * MAX_DIFFICULTY_SPEED_BOOST;
            speed *= -1;  // enemy words travel the opposite direction. 
            verticalMovement = Random.value * combatModifiers.MiniGameSpeed;
            currentDirection = Random.value >= 0.5f ? 1 : -1;
            verticalAgility = Random.value / 1000;
        }
        transform.localScale *= size;
        return this;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (verticalMovement > 0.5) {
            if (transform.localPosition.y > 50) {
                currentDirection = -1;
            } else if (transform.localPosition.y < -50) {
                currentDirection = 1;
            }
            float newHeight = transform.localPosition.y + (verticalMovement * Time.deltaTime * 20 * currentDirection); 
            transform.localPosition = new Vector3(transform.localPosition.x, newHeight, transform.localPosition.z);
        }
        //transform.SetPositionAndRotation(transform.position, Quaternion.RotateTowards(Quaternion.identity, Quaternion.Inverse(Quaternion.identity), Time.fixedDeltaTime));
        //transform.Rotate(new Vector3(1, 0, 0), 3.4f * Time.fixedDeltaTime);
        //transform.RotateAround(transform.localPosition, Vector3.forward, 20 * Time.deltaTime);
    }
}
