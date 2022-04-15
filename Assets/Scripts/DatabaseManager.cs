using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;

public class DatabaseManager
{
    static DatabaseManager db;

    List<Trait> conversationTraits;
    List<Trait> wantedTraits;
    Trait bossTrait;
    List<ItemInfo> items;
    List<GossipInfo> gossips;
    List<Conversation> conversations;
    Dictionary<Sin, EnemyData> enemyData;
    List<BattleLine> battleLines;
    List<FetchInfo> fetches;

    private DatabaseManager()
    {
        LoadItems();
        LoadGossips();
        LoadTraits();
        LoadConversations();
        LoadEnemyData();
        LoadBattleLines();
        LoadFetches();
        ;
    }

    public static DatabaseManager Instance
    {
        get { return db ?? (db = new DatabaseManager()); }
    }

    public List<Trait> ConversationTraits { get => conversationTraits; set => conversationTraits = value; }
    public List<ItemInfo> Items { get => items; set => items = value; }
    public List<GossipInfo> Gossips { get => gossips; set => gossips = value; }
    public List<Trait> WantedTraits { get => wantedTraits; set => wantedTraits = value; }
    public Trait BossTrait { get => bossTrait; set => bossTrait = value; }
    public List<Conversation> Conversations { get => conversations; set => conversations = value; }
    public Dictionary<Sin, EnemyData> EnemyData { get => enemyData; set => enemyData = value; }
    public List<BattleLine> BattleLines { get => battleLines; set => battleLines = value; }
    public List<FetchInfo> Fetches { get => fetches; set => fetches = value; }

    public List<Conversation> GetConversationsForDay(int day)
    {
        List<Conversation> dayConversations = new List<Conversation>();
        if (day < 5)
        {
            int split = Conversations.Count / 4;
            int start = split * (day - 1);
            int end = day == 4 ? Conversations.Count - 1 : split * day;


            for (int i = start; i < end; i++)
            {
                dayConversations.Add(Conversations[i]);
            }
        }
        return dayConversations;
    }

    public List<Conversation> GetConversationsForBoss()
    {
        List<Conversation> bossConversations = new List<Conversation>();
        bossConversations.Add(Conversations[Conversations.Count - 1]);

        return bossConversations;
    }

    private void LoadFetches()
    {
        List<FetchInfo> temp = new List<FetchInfo>();
        using (XmlReader read = XmlReader.Create(@"Assets/Database/Fetches.xml"))
        {
            read.ReadToFollowing("fetch");
            do
            {
                read.ReadToFollowing("subject");
                string subject = read.ReadElementContentAsString();
                read.ReadToFollowing("objective");
                string objective = read.ReadElementContentAsString();
                read.ReadToFollowing("poi");
                string poi = read.ReadElementContentAsString();
                read.ReadToFollowing("item");
                string item = read.ReadElementContentAsString();
                FetchInfo fetchInfo = new FetchInfo(subject, objective, poi, item);

                temp.Add(fetchInfo);

            } while (read.ReadToFollowing("fetch"));
        }

        Fetches = temp;
    }
    private void LoadBattleLines()
    {
        List<BattleLine> temp = new List<BattleLine>();
        using (XmlReader read = XmlReader.Create(@"Assets/Database/BattleLines.xml"))
        {
            read.ReadToFollowing("battleline");
            do
            {
                read.ReadToFollowing("sin");
                Enum.TryParse(read.ReadElementContentAsString(), out Sin sin);
                read.ReadToFollowing("text");
                string text = read.ReadElementContentAsString();
                BattleLine battleLine = new BattleLine(sin, text);

                temp.Add(battleLine);

            } while (read.ReadToFollowing("battleline"));
        }

        BattleLines = temp;
    }
    public List<string> GetBattleLines(Sin sin)
    {
        return BattleLines.Where(s => s.Sin == sin).Select(s => s.Text).ToList<string>();
    }

    private void LoadEnemyData()
    {
        Dictionary<Sin, EnemyData> temp = new Dictionary<Sin, EnemyData>();
        using (XmlReader read = XmlReader.Create(@"Assets/Database/Enemies.xml"))
        {
            read.ReadToFollowing("enemy");
            do
            {
                read.ReadToFollowing("name");
                string name = read.ReadElementContentAsString();
                read.ReadToFollowing("sin");
                Enum.TryParse(read.ReadElementContentAsString(), out Sin sin); 
                read.ReadToFollowing("sprite");
                string sprite = read.ReadElementContentAsString();
                read.ReadToFollowing("iscoworker");
                bool isCoworker = read.ReadElementContentAsBoolean();
                EnemyData enemyData = new EnemyData(name, sin, sprite, isCoworker);

                temp.Add(sin, enemyData);

            } while (read.ReadToFollowing("enemy"));
        }

        EnemyData = temp;
    }

    private void LoadConversations()
    {
        List<Conversation> temp = new List<Conversation>();
        using (XmlReader read = XmlReader.Create(@"Assets/Database/Conversations.xml"))
        {
            read.ReadToFollowing("conversation");
            do
            {
                read.ReadToFollowing("text");
                string text = read.ReadElementContentAsString();
                read.ReadToFollowing("bestresponse");
                Enum.TryParse(read.ReadElementContentAsString(), out Emoji best);
                read.ReadToFollowing("goodresponse");
                Enum.TryParse(read.ReadElementContentAsString(), out Emoji good);
                read.ReadToFollowing("worstresponse");
                Enum.TryParse(read.ReadElementContentAsString(), out Emoji worst);
                Conversation conversation = new Conversation(text, best, good, worst);

                temp.Add(conversation);

            } while (read.ReadToFollowing("conversation"));
        }

        Conversations = temp;
        Conversations.Sort(delegate (Conversation x, Conversation y) {
            return x.Text.Length.CompareTo(y.Text.Length);
        });
    }
    private void LoadItems()
    {
        List<ItemInfo> temp = new List<ItemInfo>();
        using (XmlReader read = XmlReader.Create(@"Assets/Database/Items.xml"))
        {
            read.ReadToFollowing("item");
            do
            {
                read.ReadToFollowing("name");
                string name = read.ReadElementContentAsString();
                read.ReadToFollowing("category");
                string category = read.ReadElementContentAsString();
                
                ItemInfo item = new ItemInfo(name, category);

                temp.Add(item);

            } while (read.ReadToFollowing("item"));
        }

        Items = temp;
    }

    private void LoadGossips()
    {
        List<GossipInfo> temp = new List<GossipInfo>();
        using (XmlReader read = XmlReader.Create(@"Assets/Database/Gossips.xml"))
        {
            read.ReadToFollowing("gossip");
            do
            {
                read.ReadToFollowing("name");
                string name = read.ReadElementContentAsString();
                read.ReadToFollowing("description");
                string description = read.ReadElementContentAsString();
                GossipInfo Gossip = new GossipInfo(name, description);

                temp.Add(Gossip);

            } while (read.ReadToFollowing("gossip"));
        }

        Gossips = temp;
    }

    private void LoadTraits()
    {
        List<Trait> tempConversation = new List<Trait>(); 
        List<Trait> tempWanted = new List<Trait>();

        using (XmlReader read = XmlReader.Create(@"Assets/Database/Traits.xml"))
        {
            read.ReadToFollowing("trait");
            do
            {
                read.ReadToFollowing("name");
                string name = read.ReadElementContentAsString();
                read.ReadToFollowing("type");
                Enum.TryParse(read.ReadElementContentAsString(), out TraitType type);
                read.ReadToFollowing("conversationTextSize");
                float conversationTextSize = read.ReadElementContentAsFloat();
                read.ReadToFollowing("conversationTextSpeed");
                float conversationTextSpeed = read.ReadElementContentAsFloat();
                read.ReadToFollowing("runAwayChance");
                double runAwayChance = read.ReadElementContentAsDouble();
                read.ReadToFollowing("gossipReward");
                int gossipReward = read.ReadElementContentAsInt();
                read.ReadToFollowing("itemReward");
                int itemReward = read.ReadElementContentAsInt();
                read.ReadToFollowing("miniGameSpeed");
                float miniGameSpeed = read.ReadElementContentAsFloat();
                read.ReadToFollowing("miniGameSize");
                float miniGameSize = read.ReadElementContentAsFloat();
                read.ReadToFollowing("numberOfConversations");
                int numberOfConversations = read.ReadElementContentAsInt();
                read.ReadToFollowing("friendshipPoints");
                double friendshipPoints = read.ReadElementContentAsDouble();
                read.ReadToFollowing("healthLoss");
                double healthLoss = read.ReadElementContentAsDouble();
                read.ReadToFollowing("category");
                string category = read.ReadElementContentAsString();



                CombatModifiers modifiers = new CombatModifiers(conversationTextSize, conversationTextSpeed, runAwayChance, gossipReward, itemReward, miniGameSpeed, miniGameSize, numberOfConversations, friendshipPoints, healthLoss);
                Trait trait = new Trait(name, type, modifiers, category);
                
                switch(type)
                {
                    case TraitType.Boss:
                        BossTrait = trait;
                        break;
                    case TraitType.Conversation:
                        tempConversation.Add(trait);
                        break;
                    case TraitType.Wanted:
                        tempWanted.Add(trait);
                        break;
                }

            } while (read.ReadToFollowing("trait"));
        }

        ConversationTraits = tempConversation;
        WantedTraits = tempWanted;
    }
}
