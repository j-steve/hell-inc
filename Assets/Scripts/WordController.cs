using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordController : MonoBehaviour
{

    public Color color;
    public float heightOffset = 1;
    public float spinSpeed = 0;
    public float size = 1 ;
    public float veriticalMovement = 0;

    public WordController Initialize()
    {
        color = Random.ColorHSV();
        heightOffset += (Random.value * 2 - 1) * 40;
        transform.position = transform.position +  new Vector3(0, heightOffset, 0);
        transform.localScale *= Random.value + 1;
        Debug.LogFormat("Offset is {0}, position is {1}", heightOffset, transform.position);
        return this;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //transform.SetPositionAndRotation(transform.position, Quaternion.RotateTowards(Quaternion.identity, Quaternion.Inverse(Quaternion.identity), Time.fixedDeltaTime));
        //transform.Rotate(new Vector3(1, 0, 0), 3.4f * Time.fixedDeltaTime);
        //transform.RotateAround(transform.localPosition, Vector3.forward, 20 * Time.deltaTime);
    }
}
