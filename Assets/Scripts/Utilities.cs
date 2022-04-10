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
        try
        {
            PlayerPrefs.SetFloat("MasterVolume", s.MasterVolume);
            PlayerPrefs.SetFloat("SoundEffectVolume", s.SoundEffectVolume);
            PlayerPrefs.SetFloat("MouseSensitivity", s.MouseSensitivity);
            PlayerPrefs.SetInt("MouseInversion", s.MouseInversion);
            PlayerPrefs.Save();
        }catch(Exception e)
        {
            ;
        }
    }

    public static float GetMasterVolume()
    {
        try
        {
            return PlayerPrefs.GetFloat("MasterVolume", .5f);
        }catch(Exception e)
        {
            ;
        }
        return 0f;
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
    string desciption;
    string sprite;

    public ItemInfo(string name, string desciption, string sprite)
    {
        this.name = name;
        this.desciption = desciption;
        this.sprite = sprite;
    }

    public string Name { get => name; set => name = value; }
    public string Desciption { get => desciption; set => desciption = value; }
    public string Sprite { get => sprite; set => sprite = value; }
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

        ;
    }

    public List<Trait> Traits { get => traits; set => traits = value; }
    public List<Conversation> Conversations { get => conversations; set => conversations = value; }
}

public class Trait
{
    string name;
    TraitType type;
    CombatModifiers modifiers;
    ItemInfo wantedItem;
    GossipInfo wantedGossip;

    public Trait(string name, TraitType type, CombatModifiers modifiers, ItemInfo wantedItem, GossipInfo wantedGossip)
    {
        this.name = name;
        this.type = type;
        this.modifiers = modifiers;
        this.wantedItem = wantedItem;
        this.wantedGossip = wantedGossip;
    }

    public string Name { get => name; set => name = value; }
    public TraitType Type { get => type; set => type = value; }
    public CombatModifiers Modifiers { get => modifiers; set => modifiers = value; }
    public ItemInfo WantedItem { get => wantedItem; set => wantedItem = value; }
    public GossipInfo WantedGossip { get => wantedGossip; set => wantedGossip = value; }
}

public class CombatModifiers
{
    float conversationTextSize;
    float conversationTextSpeed;
    double runAwayChance;
    int gossipReward;
    int itemReward;
    float miniGameSpeed;
    float miniGameSize;
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

public enum Emoji
{
    Happy = 0, Love = 1, Neutral = 2, Crying = 3, Shocked = 4, Anger = 5
}

public enum TraitType
{
    Conversation = 0, Wanted = 1, Boss = 2
}