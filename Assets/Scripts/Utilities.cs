using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities
{
    public static int GetDailySeed()
    {
        string date = DateTime.Now.ToShortDateString();
        return Convert.ToInt32(date.Replace("/", ""));
    }

    public static void SetSettings(Settings s)
    {
        PlayerPrefs.SetFloat("MasterVolume", s.MasterVolume);
        PlayerPrefs.SetFloat("SoundEffectVolume", s.SoundEffectVolume);
        PlayerPrefs.SetFloat("MouseSensitivity", s.MouseSensitivity);
        PlayerPrefs.SetInt("MouseInversion", s.MouseInversion);
        PlayerPrefs.Save();
    }

    public static float GetMasterVolume()
    {
        return PlayerPrefs.GetFloat("MasterVolume", .5f);
    }

    public static float GetSoundEffectVolume()
    {
        return PlayerPrefs.GetFloat("SoundEffectVolume", .5f);
    }

    public static float GetMouseSensitivity()
    {
        return PlayerPrefs.GetFloat("MouseSensitivity", .5f);
    }
    public static int GetMouseInversion()
    {
        return PlayerPrefs.GetInt("MouseInversion", 0);
    }
}

public class Settings
{
    float masterVolume;
    float soundEffectVolume;
    float mouseSensitivity;
    int mouseInversion;

    public Settings(float masterVolume, float soundEffectVolume, float mouseSensitivity, int mouseInversion)
    {
        this.masterVolume = masterVolume;
        this.soundEffectVolume = soundEffectVolume;
        this.mouseSensitivity = mouseSensitivity;
        this.mouseInversion = mouseInversion;
    }

    public float MasterVolume { get => masterVolume; set => masterVolume = value; }
    public float SoundEffectVolume { get => soundEffectVolume; set => soundEffectVolume = value; }
    public float MouseSensitivity { get => mouseSensitivity; set => mouseSensitivity = value; }
    public int MouseInversion { get => mouseInversion; set => mouseInversion = value; }
}

public class ItemInfo
{
    string name;
    string category;

    public ItemInfo(string name, string category)
    {
        this.name = name;
        this.category = category;
    }

    public string Name { get => name; set => name = value; }
    public string Category { get => category; set => category = value; }
}

public class FetchInfo
{
    string subject;
    string objective;
    string poi;
    string item;

    public FetchInfo(string subject, string objective, string poi, string item)
    {
        this.subject = subject;
        this.objective = objective;
        this.poi = poi;
        this.item = item;
    }

    public string Subject { get => subject; set => subject = value; }
    public string Objective { get => objective; set => objective = value; }
    public string Poi { get => poi; set => poi = value; }
    public string Item { get => item; set => item = value; }
}

public class GossipInfo
{
    string name;
    string desciption;

    public GossipInfo(string name, string desciption)
    {
        this.name = name;
        this.desciption = desciption;
    }

    public string Name { get => name; set => name = value; }
    public string Desciption { get => desciption; set => desciption = value; }
}

public class EnemyInfo
{
    List<Trait> traits;
    List<Conversation> conversations;

    public EnemyInfo()
    {
        Traits = new List<Trait>();
        Conversations = new List<Conversation>();

        UnityEngine.Random.InitState(Utilities.GetDailySeed());
        int randomConversation = UnityEngine.Random.Range(0, DatabaseManager.Instance.ConversationTraits.Count);
        int randomWanted = UnityEngine.Random.Range(0, DatabaseManager.Instance.WantedTraits.Count);
        Traits.Add(DatabaseManager.Instance.ConversationTraits[randomConversation]);
        Traits.Add(DatabaseManager.Instance.WantedTraits[randomWanted]);
        List<int> randomNumbers = new List<int>();

        int numOfConversations = 3;

        do
        {
            int random = UnityEngine.Random.Range(0, DatabaseManager.Instance.Conversations.Count);

            if(!randomNumbers.Contains(random))
            {
                Conversations.Add(DatabaseManager.Instance.Conversations[random]);
                randomNumbers.Add(random);
                numOfConversations--;
            }
        } while (numOfConversations > 0);
    }

    public List<Trait> Traits { get => traits; set => traits = value; }
    public List<Conversation> Conversations { get => conversations; set => conversations = value; }

    public string GetCombatLine()
    {
        return "What do you want pipsqueak?";
    }

    public Trait GetCombatTrait()
    {
        foreach(Trait t in Traits)
        {
            if (t.Type == TraitType.Conversation)
                return t;
        }
        return null;
    }
}

public class Trait
{
    string name;
    TraitType type;
    CombatModifiers modifiers;
    string category;

    public Trait(string name, TraitType type, CombatModifiers modifiers, string category)
    {
        this.name = name;
        this.type = type;
        this.modifiers = modifiers;
        this.category = category;
    }

    public string Name { get => name; set => name = value; }
    public TraitType Type { get => type; set => type = value; }
    public CombatModifiers Modifiers { get => modifiers; set => modifiers = value; }
    public string Category { get => category; set => category = value; }
}

public class CombatModifiers
{
    float conversationTextSize;
    float conversationTextSpeed;
    double runAwayChance;
    int gossipReward;
    int itemReward;
    float miniGameSpeed; //This is the vertical rate of movement
    float miniGameSize; //This is the vertical range of where text can come in from
    int numberOfConversations;
    double friendshipPoints;
    double healthLoss;

    public CombatModifiers(float conversationTextSize, float conversationTextSpeed, double runAwayChance, int gossipReward, int itemReward, float miniGameSpeed, float miniGameSize, int numberOfConversations, double friendshipPoints, double healthLoss)
    {
        this.conversationTextSize = conversationTextSize;
        this.conversationTextSpeed = conversationTextSpeed;
        this.runAwayChance = runAwayChance;
        this.gossipReward = gossipReward;
        this.itemReward = itemReward;
        this.miniGameSpeed = miniGameSpeed;
        this.miniGameSize = miniGameSize;
        this.numberOfConversations = numberOfConversations;
        this.friendshipPoints = friendshipPoints;
        this.healthLoss = healthLoss;
    }

    public float ConversationTextSize { get => conversationTextSize; set => conversationTextSize = value; }
    public float ConversationTextSpeed { get => conversationTextSpeed; set => conversationTextSpeed = value; }
    public double RunAwayChance { get => runAwayChance; set => runAwayChance = value; }
    public int GossipReward { get => gossipReward; set => gossipReward = value; }
    public int ItemReward { get => itemReward; set => itemReward = value; }
    public float MiniGameSpeed { get => miniGameSpeed; set => miniGameSpeed = value; }
    public float MiniGameSize { get => miniGameSize; set => miniGameSize = value; }
    public int NumberOfConversations { get => numberOfConversations; set => numberOfConversations = value; }
    public double FriendshipPoints { get => friendshipPoints; set => friendshipPoints = value; }
    public double HealthLoss { get => healthLoss; set => healthLoss = value; }

    public static CombatModifiers CombineModifiers(CombatModifiers x, CombatModifiers y)
    {
        return new CombatModifiers(x.ConversationTextSize * y.ConversationTextSize, x.ConversationTextSpeed * y.ConversationTextSpeed, x.RunAwayChance * y.RunAwayChance, x.GossipReward + y.GossipReward, x.ItemReward + y.ItemReward, x.MiniGameSpeed * y.MiniGameSpeed, x.MiniGameSize * y.MiniGameSize, x.NumberOfConversations + y.NumberOfConversations, x.FriendshipPoints * y.FriendshipPoints, x.HealthLoss * y.HealthLoss);
    }
}

public class Conversation
{
    string text;
    Emoji bestResponse;
    Emoji goodResponse;
    Emoji worstResponse;

    public Conversation(string text, Emoji bestResponse, Emoji goodResponse, Emoji worstResponse)
    {
        this.text = text;
        this.bestResponse = bestResponse;
        this.goodResponse = goodResponse;
        this.worstResponse = worstResponse;
    }

    public string Text { get => text; set => text = value; }
    public Emoji BestResponse { get => bestResponse; set => bestResponse = value; }
    public Emoji GoodResponse { get => goodResponse; set => goodResponse = value; }
    public Emoji WorstResponse { get => worstResponse; set => worstResponse = value; }
}

public class EnemyData
{
    string name;
    Sin sin;
    string sprite;
    bool isCoworker;

    public EnemyData(string name, Sin sin, string sprite, bool isCoworker)
    {
        this.name = name;
        this.sin = sin;
        this.sprite = sprite;
        this.isCoworker = isCoworker;
    }

    public string Name { get => name; set => name = value; }
    public Sin Sin { get => sin; set => sin = value; }
    public string Sprite { get => sprite; set => sprite = value; }
    public bool IsCoworker { get => isCoworker; set => isCoworker = value; }
}

public class BattleLine
{
    Sin sin;
    string text;

    public BattleLine(Sin sin, string text)
    {
        this.sin = sin;
        this.text = text;
    }

    public Sin Sin { get => sin; set => sin = value; }
    public string Text { get => text; set => text = value; }
}

public enum Sin
{
    Pride = 0, Gluttony = 1, Lust = 2, Wrath = 3, Envy = 4, Greed = 5
}

public enum Emoji
{
    Happy = 0, Love = 1, Neutral = 2, Crying = 3, Shocked = 4, Anger = 5
}

public enum TraitType
{
    Conversation = 0, Wanted = 1, Boss = 2
}