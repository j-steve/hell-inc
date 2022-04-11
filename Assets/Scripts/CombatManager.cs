using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    static List<string> convoResponsePrompts = new List<string>() {"What do you think of that?", "Eh?", "Know what I mean?", "What do you think?", "Can you believe it?", "What do you say to that?", "How do you feel about that?"};

    public Enemy enemy;
    public int textSpeed = 5;
    Player player = new Player();
    public bool enemyMove;
    public TextMeshProUGUI enemyName;
    public Image enemyLineImage;
    public TextMeshProUGUI enemyLine;
    public GameObject combatMenu;
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
                Debug.LogFormat("Displaying {0} adding to {1}, ts {2}, tst{3}", battleLine.Substring(0, battleLinePosition), enemyLine.text, textSpeed, textSpeedTrack);
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
