using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyInfo enemyInfo;
    public Sin sin;
    public new string name;
    public EnemyData enemyData;
    public int relationshipPoints;

    // Start is called before the first frame update
    void Start()
    {
        enemyInfo = new EnemyInfo();
        enemyData = DatabaseManager.Instance.EnemyData.Where(e => e.Key == sin).Select(e => e.Value).SingleOrDefault();
    }

    public Conversation GetRandomConversation()
    {
        return enemyInfo.Conversations.GetRandom();
    }
}
