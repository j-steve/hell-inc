using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyInfo enemyInfo;
    public Sin sin;
    public new string name;
    public EnemyData enemyData;

    // Start is called before the first frame update
    void Start()
    {
        enemyInfo = new EnemyInfo();
        enemyData = DatabaseManager.Instance.EnemyData.Where(e => e.Key == sin).Select(e => e.Value).Single();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
