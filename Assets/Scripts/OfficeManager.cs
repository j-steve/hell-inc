using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OfficeManager : MonoBehaviour
{
    public Enemy Pride;
    public Enemy Lust;
    public Enemy Gluttony;
    public Enemy Greed;
    public Enemy Wrath;
    public Enemy Envy;
    public List<Enemy> Coworkers;
    public CombatManager combatManager;
    public GameObject combatScene;
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<AudioSource>().volume = Utilities.GetMasterVolume();
        Coworkers.Add(Pride);
        Coworkers.Add(Lust);
        Coworkers.Add(Gluttony);
        Coworkers.Add(Greed);
        Coworkers.Add(Wrath);
        Coworkers.Add(Envy);
    }

    public void InitiateCombat(string enemyName)
    {
        /*switch(enemyName)
        {
            case "Asmo":
                CombatManager.enemy = Lust;
                break;
            case "Lou":
                CombatManager.enemy = Pride;
                break;
            case "BeelzeBob":
                CombatManager.enemy = Envy;
                break;
            case "Sathy":
                CombatManager.enemy = Wrath;
                break;
            case "Belphie":
                CombatManager.enemy = Gluttony;
                break;
            case "Mammon":
                CombatManager.enemy = Lust;
                break;
        }
        CombatManager.needsReset = true;
        CombatManager.player = player;
        DontDestroyOnLoad(player);
        DontDestroyOnLoad(Lust);
        DontDestroyOnLoad(Pride);
        DontDestroyOnLoad(Envy);
        DontDestroyOnLoad(Wrath);
        DontDestroyOnLoad(Gluttony);
        SceneManager.LoadScene("Combat");*/
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
