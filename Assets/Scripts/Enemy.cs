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
    public int relationshipPoints = 0;
    public List<BattleLine> BattleLines;

    // Start is called before the first frame update
    void Start()
    {
        enemyInfo = new EnemyInfo();
        enemyInfo.LoadConversations(1);//This is the day
        enemyData = DatabaseManager.Instance.EnemyData.Where(e => e.Key == sin).Select(e => e.Value).SingleOrDefault();

        BattleLines = DatabaseManager.Instance.BattleLines.Where(b => b.Sin == sin).ToList();
    }

    public string GetCombatLine()
    {
        return BattleLines.GetRandom().Text;
    }

    public Conversation GetRandomConversation()
    {
        return enemyInfo.Conversations.GetRandom();
    }
}
