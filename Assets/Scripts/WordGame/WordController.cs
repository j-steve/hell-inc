using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordController : MonoBehaviour
{

    public Color color;
    public float heightOffset = 1;
    public float spinSpeed = 0;
    public float size = 1 ;
    public float speed = 1;
    public float verticalMovement = 0;
    public float verticalAgility = 0;
    public int currentDirection = 1;

    public WordController Initialize(CombatModifiers combatModifiers)
    {
        color = Random.ColorHSV();
        heightOffset += (Random.value * 2 - 1) * 40 * combatModifiers.MiniGameSize;
        transform.position = transform.position +  new Vector3(0, heightOffset, 0);
        transform.localScale *= (Random.value + 1) * combatModifiers.ConversationTextSize;
        speed = Random.value * combatModifiers.ConversationTextSpeed;
        verticalMovement = Random.value * combatModifiers.MiniGameSpeed;
        currentDirection = Random.value >= 0.5f ? 1 : -1;
        verticalAgility = Random.value / 1000;
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
