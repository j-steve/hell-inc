using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BulletWordRandomizer : MonoBehaviour
{
    public string[] words;
    // Start is called before the first frame update
    void Start()
    {
        ChooseWord();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChooseWord()
    {
        int rand = Random.Range(0,words.Length -1);
        GetComponent<TextMeshProUGUI>().text = words[rand];
    }

    
}
