using System;
using System.Collections;
using System.Collections.Generic;
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

    private DatabaseManager()
    {
        LoadItems();
        LoadGossips();
        LoadTraits();
        loadConversations();
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

    private void loadConversations()
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
                read.ReadToFollowing("description");
                string description = read.ReadElementContentAsString();
                read.ReadToFollowing("sprite");
                string sprite = read.ReadElementContentAsString();
                ItemInfo item = new ItemInfo(name, description, sprite);

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
                read.ReadToFollowing("item");
                ItemInfo wantedItem = null;
                string text = read.ReadElementContentAsString();
                if(text != "")
                    wantedItem = Items.Find(i => i.Name == text);

                read.ReadToFollowing("gossip");
                GossipInfo wantedGossip = null;
                text = read.ReadElementContentAsString();
                if (text != "")
                    wantedGossip = Gossips.Find(g => g.Name == text);

                CombatModifiers modifiers = new CombatModifiers(conversationTextSize, conversationTextSpeed, runAwayChance, gossipReward, itemReward, miniGameSpeed, miniGameSize, numberOfConversations, friendshipPoints, healthLoss);
                Trait trait = new Trait(name, type, modifiers, wantedItem, wantedGossip);
                
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
