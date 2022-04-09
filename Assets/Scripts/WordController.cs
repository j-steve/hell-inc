using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordController : MonoBehaviour
{
    public static WordController Create(WordController prefab, Transform parent)
    {
        WordController word = Instantiate(prefab, parent);
        word.color = Random.ColorHSV();
        return word;
    }

    public Color color;
    public float heightOffset =0;
    public float spinSpeed= 0 ;
    public float size = 1 ;
    public float veriticalMovement = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
