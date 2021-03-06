using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{

    const int TEXT_SPEED = 1;
    /** How long to display the outcome of the emoji selection, in seconds, before returning to the main screen. */
    const float EMOJI_OUTCOME_DISPLAY_SECONDS = 3f;
    static List<string> convoResponsePrompts = new List<string>() { "What do you think of that?", "How do you feel about that?", "Eh?", "Know what I mean?", "What do you think?", "Can you believe it?", "What do you say to that?", };
    static List<string> convoFailLines = new List<string>() { "Did you fall asleep while I was talking??", "Hey, hey wake up!! I'm talking here!" };
    static List<string> convoContinuePrompts = new List<string>() { "Anyways, as I was saying...", "What's next...", "I could go on...", "But, there was something else I wanted to tell you...", "Anyways...", "Let's talk about something else.", "I wanted to talk about something else, though."};
    static Dictionary<EmojiRating, List<string>> emojiRatingLines = new Dictionary<EmojiRating, List<string>>() {
        { EmojiRating.BEST, new List<string>() { "Exactly!!", "I feel the same way!", "Totally, I knew you'd get it!", "Agreed!" } },
        { EmojiRating.GOOD, new List<string>() { "Yeah, pretty much.", "Basically.", "Hm, I can see how you'd feel that way.", "I suppose so.", "Yeah, that's fair.", "Hm, I guess so" } },
        { EmojiRating.BAD, new List<string>() { "I don't think you were even listening.", "That doesn't make sense to me.", "What? Why would you say that?", "Were you listening to me at all?", "Hm, I don't think we're on the same page there.", "I can't say I agree with that." } },
        { EmojiRating.WORST, new List<string>() {  "WHAT? How could you say that??", "Are you kidding me??", "What are you, crazy?", "What? That doesn't make any sense!", "You weren't listening at all!" } }
    };
    static Dictionary<EmojiRating, Color> emojiRatingColors = new Dictionary<EmojiRating, Color>() {
        {EmojiRating.BEST, new Color32(5, 173, 7, 255)}, // Bright Green
        {EmojiRating.GOOD, new Color32(159, 188, 155, 255)}, // Pale Green
        {EmojiRating.BAD, new Color32(229, 229, 229, 121)}, // Gray
        {EmojiRating.WORST, new Color32(255, 117, 117, 255)} // Red
    };
    static Dictionary<Emoji, int> emojiUnicodeMap = new Dictionary<Emoji, int> { { Emoji.Happy, 0x1F600 }, { Emoji.Love, 0x1F601 }, { Emoji.Neutral, 0x1F609 }, { Emoji.Crying, 0x1F603 }, { Emoji.Shocked, 0x1F606 }, { Emoji.Anger, 0x1F605 } };

    public Enemy enemy;
    public GameObject enemySprite;
    public Player player;
    public bool enemyMove;
    public TextMeshProUGUI enemyName;
    public TextMeshProUGUI enemyTrait;
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
    public Button CombatFailOkButton;
    public Transform ItemInventoryContainer;
    string lastSelectedItem;
    bool displayText;
    string battleLine;
    int battleLinePosition;
    int textSpeedTrack;
    float? returnToMainMenuTime = null;
    int preCombatRelationsip;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Enemy?.enemyName == null) {
            enemy = new Enemy().Initialize(((IEnumerable<Sin>)Enum.GetValues(typeof(Sin))).GetRandom());
        } else {
            enemy = GameManager.Enemy;
            enemy.enemyInfo.LoadConversations(GameManager.WorkDay);
        }
        for (int i = 0; i < enemySprite.transform.childCount; i++) {
            if (enemySprite.transform.GetChild(i).name == enemy.enemyName) {
                enemySprite.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        gameObject.GetComponentsInParent<AudioSource>()[0].volume = Utilities.GetMasterVolume();
        gameObject.GetComponentsInParent<AudioSource>()[0].Play();
        player.LockPlayer = true;
        preCombatRelationsip = enemy.relationshipPoints;
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
        CombatFailOkButton.onClick.AddListener(GameManager.EndDay);
        wordGameController.OnGameWon += (delegate () {
            if (GameManager.WorkDay == 5)
            {
                GameManager.EndDay();
            }
            else
            {
                SetBattleLine(convoResponsePrompts.GetRandom());
                encounterStartMenu.SetActive(false);
                conversationResponseMenu.SetActive(true);
                combatMenu.SetActive(true);
            }
        });
        wordGameController.OnGameLost += (delegate () {
            SetBattleLine(convoFailLines.GetRandom());
            encounterStartMenu.SetActive(false);
            conversationFailMenu.SetActive(true);
            combatMenu.SetActive(true);
        });

        //List<ItemInfo> items = DatabaseManager.Instance.Items;

        //player.AddItem(Utilities.GetRandomItem());
    }

    void SetBattleLine(string battleLine)
    {
        this.battleLine = battleLine;
        battleLinePosition = 0;
        enemyLine.text = "";
        enemyMove = true;
        displayText = false;
        battleLinePosition = 0;
        textSpeedTrack = TEXT_SPEED;
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
        EmojiRating emojiRating = currentConversation.GetEmojiRating(clickedEmoji);
        
        // Disable all emoji buttons, and highlight the correct emoji response.
        foreach(Button emojiButton in conversationResponseMenu.GetComponentsInChildren<Button>()) {
            emojiButton.interactable = false;
            Emoji thisEmoji = (Emoji)Enum.Parse(typeof(Emoji), emojiButton.name);
            ColorBlock newColorBlock = emojiButton.colors;
            newColorBlock.disabledColor = emojiRatingColors[currentConversation.GetEmojiRating(thisEmoji)];
            emojiButton.colors = newColorBlock;
        }

        if(emojiRating == EmojiRating.BEST)
        {
            enemy.relationshipPoints += 50;
        }
        else if (emojiRating == EmojiRating.GOOD)
        {
            enemy.relationshipPoints += 25;
        }
        else if (emojiRating == EmojiRating.WORST)
        {
            if (enemy.relationshipPoints % 100 >= 25)
                enemy.relationshipPoints -= 25;
            else
                enemy.relationshipPoints = ((enemy.relationshipPoints / 100) * 100);
        }

        float levelUps = ((enemy.relationshipPoints / 100) - (preCombatRelationsip / 100));
        
        switch(enemy.sin)
        {
            case Sin.Envy:
                player.Modifiers.Envy += (levelUps / 10.0f);
                break;
            case Sin.Gluttony:
                player.Modifiers.Gluttony += (levelUps / 10.0f);
                break;
            case Sin.Greed:
                player.Modifiers.Greed += (levelUps / 10.0f);
                break;
            case Sin.Lust:
                player.Modifiers.Lust += (levelUps / 10.0f);
                break;
            case Sin.Wrath:
                player.Modifiers.Wrath += (levelUps / 10.0f);
                break;
        }

        string extra = "";

        if(levelUps > 0)
            extra += " [You feel a little stronger.]";

        if (enemy.relationshipPoints >= 200 && !enemyTrait.gameObject.activeSelf)
        {
            enemyTrait.text = "Likes " + enemy.enemyInfo.GetWantedtTrait().Category;
            extra = " [You now know something about " + enemy.enemyName + "]";
            enemyTrait.gameObject.SetActive(true);
        }
        SetBattleLine(emojiRatingLines[emojiRating].GetRandom() + extra);
        returnToMainMenuTime = Time.time + EMOJI_OUTCOME_DISPLAY_SECONDS;
    }
    
    public void ButtonOnClick(Button b)
    {
        TextMeshProUGUI textmeshPro = b.GetComponentInChildren<TextMeshProUGUI>();
        lastSelectedItem = textmeshPro.text;
    }

    public void SelectItem()
    {
        if (lastSelectedItem != "")
        {
            Debug.Log(lastSelectedItem);
            ItemInventory.SetActive(false);
            GameManager.Player.RemoveItem(lastSelectedItem);

            if (enemy.enemyInfo.GetWantedtTrait().Category == DatabaseManager.Instance.Items.Find(i => i.Name == lastSelectedItem).Category)
            {
                string extra = "";
                int points = enemy.relationshipPoints;
                enemy.relationshipPoints += 50;
                if((enemy.relationshipPoints / 100) > (preCombatRelationsip / 100))
                {
                    switch (enemy.sin)
                    {
                        case Sin.Envy:
                            player.Modifiers.Envy += .1f;
                            break;
                        case Sin.Gluttony:
                            player.Modifiers.Gluttony += .1f;
                            break;
                        case Sin.Greed:
                            player.Modifiers.Greed += .1f;
                            break;
                        case Sin.Lust:
                            player.Modifiers.Lust += .1f;
                            break;
                        case Sin.Wrath:
                            player.Modifiers.Wrath += .1f;
                            break;
                    }
                    preCombatRelationsip += 50;
                    extra += " You feel a little stronger.";
                    
                    if ((enemy.relationshipPoints / 100) == 2)
                    {
                        enemyTrait.text = "Likes " + enemy.enemyInfo.GetWantedtTrait().Category;
                        extra = " [You now know something about " + enemy.enemyName + "]";
                        enemyTrait.gameObject.SetActive(true);
                    }
                }
                SetBattleLine("Wow, I love it!" + extra);
            }
            else
            {
                SetBattleLine("Wow, I hate it!");
            }
            lastSelectedItem = "";
        }
    }

    public void BackFromInventory()
    {
        ItemInventory.SetActive(false);
        lastSelectedItem = "";
    }

    public void GiveItem()
    {
        if (GameManager.Player.ItemInventory == null)
            GameManager.Player.ItemInventory = new List<ItemInfo>();

        foreach (Transform child in ItemInventoryContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (ItemInfo i in GameManager.Player.ItemInventory)
        {
            Button b = Instantiate(ItemSlotPrefab);
            TextMeshProUGUI textmeshPro = b.GetComponentInChildren<TextMeshProUGUI>();
            textmeshPro.text = i.Name;
            b.onClick.AddListener(delegate { ButtonOnClick(b); });

            b.transform.SetParent(ItemInventoryContainer, false);
        }
        ItemInventory.SetActive(true);
    }

    public void TellGossip()
    {

    }

    public void Run()
    {
        int result = UnityEngine.Random.Range(0, 100);
        if((enemy.enemyInfo.GetCombatTrait().Modifiers.RunAwayChance * 50) > result)
        {
            GameManager.ReturnToOffice();
        }
        else
        {
            GameManager.Player.AttentionSpanCurrent -= .1f;
            SetBattleLine("You're not getting away that easily");
            if (GameManager.Player.AttentionSpanCurrent <= 0)
            {
                SetBattleLine(convoFailLines.GetRandom());
                encounterStartMenu.SetActive(false);
                conversationFailMenu.SetActive(true);
                combatMenu.SetActive(true);
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyMove)
        {
            enemySprite.transform.position = new Vector3(enemySprite.transform.position.x - 2, enemySprite.transform.position.y, enemySprite.transform.position.z);
            if (enemySprite.transform.position.x <= 70)
            {
                enemyMove = false;
                enemyName.text = enemy.enemyName;
                if(enemy.relationshipPoints >= 200)
                {
                    enemyTrait.text = "Likes " + enemy.enemyInfo.GetWantedtTrait().Category;
                    enemyTrait.gameObject.SetActive(true);
                }
                enemyName.gameObject.SetActive(true);
                enemyLineImage.gameObject.SetActive(true);
                displayText = true;
            }
        }
        else if(displayText)
        {
            if(TEXT_SPEED == textSpeedTrack)
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
            if(enemy.enemyInfo.Conversations.Count == 0)
            {
                GameManager.ReturnToOffice();
            }

            foreach (Button button in conversationResponseMenu.GetComponentsInChildren<Button>()) { button.interactable=true; }
            conversationResponseMenu.SetActive(false);
            encounterStartMenu.SetActive(true);
            SetBattleLine(convoContinuePrompts.GetRandom());
            returnToMainMenuTime = null;
        }
    }
}
