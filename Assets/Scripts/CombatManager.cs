using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    /** How long to display the outcome of the emoji selection, in seconds, before returning to the main screen. */
    const float EMOJI_OUTCOME_DISPLAY_SECONDS = 1.5f;
    static List<string> convoResponsePrompts = new List<string>() {"What do you think of that?", "How do you feel about that?", "Eh?", "Know what I mean?", "What do you think?", "Can you believe it?", "What do you say to that?", };
    static List<string> convoFailLines = new List<string>() { "Did you fall asleep while I was talking??",  "Hey, hey wake up!! I'm talking here!"};
    static List<string> convoContinuePrompts = new List<string>() { "Anyways, as I was saying...", "What's next...",  "I could go on...", "But, there was something else I wanted to tell you...", "Anyways...", "Let's talk about something else.", "I wanted to talk about something else though...", "I feel the same way!"};
    static List<string> convoBestResponse = new List<string>() { "Exactly!!", "I feel the same way!", "Totally, I knew you'd get it!", "Agreed!" };
    static List<string> convoGoodResponse = new List<string>() { "Yeah, pretty much.", "Basically.", "Hm, I can see how you'd feel that way.", "I suppose so.", "Yeah, that's fair.", "Hm, I guess so"};
    static List<string> convoBadResponse = new List<string>() { "I don't think you were even listening.", "That doesn't make sense to me.", "What? Why would you say that?", "Were you listening to me at all?", "Hm, I don't think we're on the same page there.", "I can't say I agree with that."};
    static List<string> convoWorstResponse = new List<String>() { "WHAT? How could you say that??", "Are you kidding me??", "What are you, crazy?", "What? That doesn't make any sense!", "You weren't listening at all!" };
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
    public GameObject conversationFailMenu;
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
    float? returnToMainMenuTime = null;

    // Start is called before the first frame update
    void Start()
    {
        wordGameController.gameObject.SetActive(false);
        combatMenu.SetActive(true);
        encounterStartMenu.SetActive(true);
        conversationResponseMenu.SetActive(false);
        conversationFailMenu.SetActive(false);
        SetBattleLine(enemy.GetCombatLine());
        CreateEmojiMenu();
        converseButton.onClick.AddListener(delegate() {
            combatMenu.SetActive(false);
            ItemInventory.SetActive(false);
            currentConversation = enemy.GetRandomConversation();
            wordGameController.Initialize(currentConversation.Text, enemy.enemyInfo.GetCombatTrait().Modifiers);
        });
        wordGameController.OnGameWon += (delegate () { 
            SetBattleLine(convoResponsePrompts.GetRandom());
            encounterStartMenu.SetActive(false);
            conversationResponseMenu.SetActive(true);
            combatMenu.SetActive(true);
        });
        wordGameController.OnGameLost += (delegate () {
            SetBattleLine(convoFailLines.GetRandom());
            encounterStartMenu.SetActive(false);
            conversationFailMenu.SetActive(true);
            combatMenu.SetActive(true);
        });

        List<ItemInfo> items = DatabaseManager.Instance.Items;

        foreach (ItemInfo i in items)//GameManager.Player.ItemInventory)
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
            if (emoji == Emoji.UNDEFINED) { continue; }
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
            SetBattleLine(convoBestResponse.GetRandom());
        }
        else if (clickedEmoji == currentConversation.GoodResponse)
        {
            SetBattleLine(convoGoodResponse.GetRandom());
        }
        else if (clickedEmoji == currentConversation.WorstResponse)
        {
            SetBattleLine(convoWorstResponse.GetRandom());
        }
        else
        {
            SetBattleLine(convoBadResponse.GetRandom());
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
        returnToMainMenuTime = Time.time + EMOJI_OUTCOME_DISPLAY_SECONDS;
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
        //GameManager.Player.RemoveItem(lastSelectedItem);

        if (enemy.enemyInfo.GetWantedtTrait().Category == DatabaseManager.Instance.Items.Find(i => i.Name == lastSelectedItem).Category)
        {
            //relationship points go up
            SetBattleLine("Wow, I love it!");
        }
        else
        {
            SetBattleLine("Wow, I hate it!");
        }
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
        } else if (returnToMainMenuTime.HasValue && Time.time >= returnToMainMenuTime) {
            foreach (Button button in conversationResponseMenu.GetComponentsInChildren<Button>()) { Destroy(button.gameObject); }
            CreateEmojiMenu();
            conversationResponseMenu.SetActive(false);
            encounterStartMenu.SetActive(true);
            SetBattleLine(convoContinuePrompts.GetRandom());
            returnToMainMenuTime = null;
        }
    }
}
