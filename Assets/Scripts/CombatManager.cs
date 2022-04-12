using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    static List<string> convoResponsePrompts = new List<string>() {"What do you think of that?", "How do you feel about that?", "Eh?", "Know what I mean?", "What do you think?", "Can you believe it?", "What do you say to that?", };
    static Dictionary<Emoji, int> emojiUnicodeMap = new Dictionary<Emoji, int> { { Emoji.Happy, 0x1F600 }, { Emoji.Love, 0x1F601 }, { Emoji.Neutral, 0x1F609 }, { Emoji.Crying, 0x1F603 }, { Emoji.Shocked, 0x1F606 }, { Emoji.Anger, 0x1F605 } };

    public Enemy enemy;
    public int textSpeed = 5;
    Player player = new Player();
    public bool enemyMove;
    public TextMeshProUGUI enemyName;
    public Image enemyLineImage;
    public TextMeshProUGUI enemyLine;
    public GameObject combatMenu;
    public Button menuButtonPrefab;
    public Button converseButton;
    public GameObject encounterStartMenu;
    public GameObject conversationResponseMenu;
    public WordGameController wordGameController;
    Conversation currentConversation;
    public GameObject ItemInventory;
    public Button ItemSlotPrefab;
    public Transform ItemInventoryContainer;
    string lastSelectedItem;
    bool displayText;
    string battleLine;
    int battleLinePosition;
    int textSpeedTrack;

    // Start is called before the first frame update
    void Start()
    {
        encounterStartMenu.SetActive(true);
        conversationResponseMenu.SetActive(false);
        SetBattleLine(enemy.GetCombatLine());
        CreateEmojiMenu();
        converseButton.onClick.AddListener(delegate() {
            combatMenu.SetActive(false);
            currentConversation = enemy.GetRandomConversation();
            wordGameController.Initialize(currentConversation.Text, player.GetCombatModifiersForEnemy(enemy));
        });
        wordGameController.OnGameWon += (delegate () { 
            SetBattleLine(convoResponsePrompts.GetRandom());
            encounterStartMenu.SetActive(false);
            conversationResponseMenu.SetActive(true);
            combatMenu.SetActive(true);
        });
        wordGameController.OnGameLost += (delegate () { 
            combatMenu.SetActive(true);
            // TODO: add word game lose behavior
        });

        List<ItemInfo> items = DatabaseManager.Instance.Items;

        foreach (ItemInfo i in items)
        {
            Button b = Instantiate(ItemSlotPrefab);
            TextMeshProUGUI textmeshPro = b.GetComponentInChildren<TextMeshProUGUI>();
            textmeshPro.text = i.Name;
            b.onClick.AddListener(delegate { ButtonOnClick(b); });


            b.transform.SetParent(ItemInventoryContainer, false);
        }
    }

    void SetBattleLine(string battleLine)
    {
        this.battleLine = battleLine;
        battleLinePosition = 0;
        enemyLine.text = "";
        enemyMove = true;
        displayText = false;
        battleLinePosition = 0;
        textSpeedTrack = textSpeed;
    }

    void CreateEmojiMenu()
    {
        foreach (Emoji emoji in Enum.GetValues(typeof(Emoji))) {
            Button emojiButton = Instantiate(menuButtonPrefab, conversationResponseMenu.transform);
            emojiButton.name = Enum.GetName(typeof(Emoji), emoji);
            emojiButton.GetComponentInChildren<TextMeshProUGUI>().text = char.ConvertFromUtf32(emojiUnicodeMap[emoji]);
            emojiButton.onClick.AddListener(delegate () { EmojiButtonClicked(emoji); });
        }
    }

    void EmojiButtonClicked(Emoji clickedEmoji)
    {
        if (clickedEmoji == currentConversation.BestResponse)
        {
            SetBattleLine("Exactly!!");
        }
        else if (clickedEmoji == currentConversation.GoodResponse)
        {
            SetBattleLine("Yeah, pretty much.");
        }
        else if (clickedEmoji == currentConversation.WorstResponse)
        {
            SetBattleLine("WHAT? How could you say that??");
        }
        else
        {
            SetBattleLine("I don't think you were even listening!");
        }
        // Disable all emoji buttons, and highlight the correct emoji response.
        foreach(Button emojiButton in conversationResponseMenu.GetComponentsInChildren<Button>()) {
            emojiButton.interactable = false;
            Emoji thisEmoji = (Emoji)Enum.Parse(typeof(Emoji), emojiButton.name);
            Color? newColor = null;
            if (thisEmoji == currentConversation.GoodResponse) {
                newColor = new Color32(159, 188, 155, 255); // Pale Green
            } else if (thisEmoji == currentConversation.BestResponse) {
                newColor = new Color32(5, 173, 7, 255); // Bright Green 
            } else if (thisEmoji == currentConversation.WorstResponse) {
                newColor = new Color32(255, 117, 117, 255); // Red
            }
            if (newColor.HasValue) {
                emojiButton.colors = new ColorBlock {normalColor = newColor.Value, disabledColor = newColor.Value, colorMultiplier = 1};
            }
        }
    }
    
    public void ButtonOnClick(Button b)
    {
        TextMeshProUGUI textmeshPro = b.GetComponentInChildren<TextMeshProUGUI>();
        lastSelectedItem = textmeshPro.text;
    }

    public void SelectItem()
    {
        Debug.Log(lastSelectedItem);
        ItemInventory.SetActive(false);
        //remove item from inventory
        lastSelectedItem = "";
    }

    public void BackFromInventory()
    {
        ItemInventory.SetActive(false);
        lastSelectedItem = "";
    }

    public void GiveItem()
    {
        ItemInventory.SetActive(true);
    }

    public void TellGossip()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (enemyMove)
        {
            enemy.transform.position = new Vector3(enemy.transform.position.x - .5f, enemy.transform.position.y, enemy.transform.position.z);
            if (enemy.transform.position.x <= 70)
            {
                enemyMove = false;
                enemyName.text = enemy.name;
                enemyName.gameObject.SetActive(true);
                enemyLineImage.gameObject.SetActive(true);
                displayText = true;
            }
        }
        else if(displayText)
        {
            if(textSpeed == textSpeedTrack)
            {
                enemyLine.text = battleLine.Substring(0, battleLinePosition);
                battleLinePosition++;
                textSpeedTrack = 0;
            }
            else
            {
                textSpeedTrack++;
            }
            if (battleLinePosition == battleLine.Length + 1)
                displayText = false;
        }
    }
}
