using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyInfo enemyInfo;
    public Sin sin;
    public string enemyName;
    public EnemyData enemyData;
    public int relationshipPoints = 0;
    public List<BattleLine> BattleLines;

    // Start is called before the first frame update
    void Start()
    {
        Initialize(sin);
    }

    public Enemy Initialize(Sin sin)
    {
        this.sin = sin;
        enemyName = getSinEnemyName(sin);
        enemyInfo = new EnemyInfo();
        enemyInfo.LoadConversations(1);//This is the day
        if (sin == Sin.Pride)
        {
            enemyInfo.Conversations = DatabaseManager.Instance.GetConversationsForBoss();
            enemyInfo.SetAsBoss();
        }
        enemyData = DatabaseManager.Instance.EnemyData.Where(e => e.Key == sin).Select(e => e.Value).SingleOrDefault();
        BattleLines = DatabaseManager.Instance.BattleLines.Where(b => b.Sin == sin).ToList();
        return this;
    }

    private static string getSinEnemyName(Sin sin)
    {
        switch (sin) {
            case Sin.Envy: return "BeelzeBob";
            case Sin.Gluttony: return "Belphie";
            case Sin.Lust: return "Asmo";
            case Sin.Greed: return "Mams";
            case Sin.Wrath: return "Sathy";
            case Sin.Pride: return "Lou";
            default: return "Aba";
        }
    }

    public string GetCombatLine()
    {
        return BattleLines.GetRandom().Text;
    }

    public Conversation GetRandomConversation()
    {
        Conversation c = enemyInfo.Conversations.GetRandom();
        enemyInfo.Conversations.Remove(c);
        return c;
    }

    internal object Initialize(IEnumerable<Sin> enumerable)
    {
        throw new NotImplementedException();
    }
}
