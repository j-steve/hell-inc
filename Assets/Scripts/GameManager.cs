using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static Dictionary<Sin, Enemy> coworkers; 

    public static Dictionary<Sin, Enemy> Coworkers 
    {
        get
        {
            if(coworkers == null)
            {
                coworkers = new Dictionary<Sin, Enemy>();

                foreach (string name in Enum.GetNames(typeof(Sin)))
                {
                    Enemy e = Instantiate(new Enemy());
                    Enum.TryParse(name, out Sin sin);
                    e.sin = sin;
                    coworkers.Add(sin, e);
                }
                
            }
            return coworkers;
        }
        
        set => coworkers = value; 

    }

    public static Player Player { get; private set; } = new Player();

    public static int WorkDay { get; private set; } = 1;

    public static event Action OnStartCombat;
    public static event Action OnCompleteCombat;
    public static event Action OnDayEnd;

    private static OfficeManager _officeManager;
    private static OfficeManager officeManager {
        get {
            if (_officeManager == null) {_officeManager = FindObjectOfType<OfficeManager>();}
            return _officeManager;
        }
    }

    public static void StartCombat()
    {
        officeManager.gameObject.SetActive(false);
        SceneManager.LoadScene("Combat", LoadSceneMode.Additive);
        OnStartCombat?.Invoke();
    }

    public static void ReturnToOffice()
    {
        SceneManager.UnloadSceneAsync("Combat");
        officeManager.gameObject.SetActive(true);
        OnCompleteCombat?.Invoke();
    }
    public static void EndDay()
    {
        SceneManager.UnloadSceneAsync("Combat");
        WorkDay += 1;
        officeManager.gameObject.SetActive(true);
        OnDayEnd?.Invoke();
    }
}
