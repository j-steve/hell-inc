using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public Enemy enemy;
    public Player player;
    public bool enemyMove;
    public TextMeshProUGUI enemyName;

    // Start is called before the first frame update
    void Start()
    {
        enemyMove = true;
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
            }
        }
        else
        {

        }
    }
}
