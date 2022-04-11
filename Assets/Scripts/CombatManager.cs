using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    static List<string> convoResponsePrompts = new List<string>() {"What do you think of that?", "Eh?", "Know what I mean?", "What do you think?", "Can you believe it?", "What do you say to that?", "How do you feel about that?"};
    static Dictionary<Emoji, int> emojiUnicodeMap = new Dictionary<Emoji, int> { { Emoji.Happy, 0x1F600 }, { Emoji.Love, 0x1F600 }, { Emoji.Neutral, 0x1F602}, { Emoji.Crying, 0x1F603}, { Emoji.Shocked, 0x1F604  }, { Emoji.Anger, 0x1F605 } };

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
    bool displayText;
    string battleLine;
    int battleLinePosition;
    int textSpeedTrack;

    // Start is called before the first frame update
    void Start()
    {
        encounterStartMenu.SetActive(true);
        conversationResponseMenu.SetActive(false);
        SetBattleLine(enemy.enemyInfo.GetCombatLine());
        CreateEmojiMenu();
        converseButton.onClick.AddListener(delegate() {
            combatMenu.SetActive(false);
            Conversation conversation = enemy.GetRandomConversation();
            wordGameController.Initialize(conversation.Text, player.GetCombatModifiersForEnemy(enemy));
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
            emojiButton.GetComponentInChildren<TextMeshProUGUI>().text = char.ConvertFromUtf32(emojiUnicodeMap[emoji]);
        }
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
