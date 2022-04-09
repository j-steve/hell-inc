using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class DatabaseManager
{
    static DatabaseManager db;

    List<Trait> conversationTraits;
    List<Trait> escapeTraits;
    Trait bossTrait;
    List<ItemInfo> items;
    List<GossipInfo> gossips;

    private DatabaseManager()
    {
        LoadItems();
        LoadGossips();
        LoadTraits();
    }

    public static DatabaseManager Instance
    {
        get { return db ?? (db = new DatabaseManager()); }
    }

    public List<Trait> ConversationTraits { get => conversationTraits; set => conversationTraits = value; }
    public List<ItemInfo> Items { get => items; set => items = value; }
    public List<GossipInfo> Gossips { get => gossips; set => gossips = value; }
    public List<Trait> EscapeTraits { get => escapeTraits; set => escapeTraits = value; }
    public Trait BossTrait { get => bossTrait; set => bossTrait = value; }

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
        List<Trait> tempEscape = new List<Trait>();

        using (XmlReader read = XmlReader.Create(@"Assets/Database/Traits.xml"))
        {
            read.ReadToFollowing("trait");
            do
            {
                read.ReadToFollowing("name");
                string name = read.ReadElementContentAsString();
                read.ReadToFollowing("type");
                string traitType = read.ReadElementContentAsString();
                TraitType type;
                switch (traitType)
                {
                    case "Boss":
                        type = TraitType.Boss;
                        break;
                    case "Conversation":
                        type = TraitType.Conversation;
                        break;
                    case "Escape":
                        type = TraitType.Escape;
                        break;
                    default:
                        type = TraitType.Conversation;
                        break;
                }
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
                    case TraitType.Escape:
                        tempEscape.Add(trait);
                        break;
                }

            } while (read.ReadToFollowing("trait"));
        }

        ConversationTraits = tempConversation;
        EscapeTraits = tempEscape;
    }
}
