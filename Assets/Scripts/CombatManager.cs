using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    public Enemy enemy;
    public int textSpeed = 5;
    Player player = new Player();
    public bool enemyMove;
    public TextMeshProUGUI enemyName;
    public Image enemyLineImage;
    public TextMeshProUGUI enemyLine;
    public GameObject combatMenu;
    public Button converseButton;
    public WordGameController wordGameController;
    bool displayText;
    string battleLine;
    int battleLinePosition;
    int textSpeedTrack;

    // Start is called before the first frame update
    void Start()
    {
        enemyMove = true;
        displayText = false;
        battleLine = enemy.enemyInfo.GetCombatLine();
        battleLinePosition = 0;
        textSpeedTrack = textSpeed;
        converseButton.onClick.AddListener(delegate() {
            combatMenu.SetActive(false);
            Conversation conversation = enemy.GetRandomConversation();
            wordGameController.Initialize(conversation.Text, player.GetCombatModifiersForEnemy(enemy));
        });
        wordGameController.OnGameWon += (delegate () { 
            combatMenu.SetActive(true);
            // TODO: add word game win behavior
        });
        wordGameController.OnGameLost += (delegate () { 
            combatMenu.SetActive(true);
            // TODO: add word game lose behavior
        });
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyMove)
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
        else
        {

        }
    }
}
