using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyInfo enemyInfo;
    public new string name;

    // Start is called before the first frame update
    void Start()
    {
        enemyInfo = new EnemyInfo();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
