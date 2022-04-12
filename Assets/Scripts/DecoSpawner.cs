using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoSpawner : MonoBehaviour
{
    public int spawnChance;
    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn()
    {
        int rand = Random.Range(0,100);
        if (rand > spawnChance)
        {
            gameObject.SetActive(false);
        }
    }
}
