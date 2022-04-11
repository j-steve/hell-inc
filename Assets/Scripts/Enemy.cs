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

    public Conversation GetRandomConversation()
    {
        int randomIndex = Random.Range(0, enemyInfo.Conversations.Count);
        return enemyInfo.Conversations[randomIndex];
    }
}
