using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts;

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

                foreach (Sin sin in Enum.GetValues(typeof(Sin)))
                { 
                    coworkers.Add(sin, new Enemy().Initialize(sin));
                }
                
            }
            return coworkers;
        }
        
        set => coworkers = value; 

    }

    public static Player Player { get; private set; } = new Player();

    public static Enemy Enemy { get; private set; }

    public static int WorkDay { get; private set; } = 1;

    public static event Action<Enemy> OnStartCombat;
    public static event Action OnCompleteCombat;
    public static event Action OnDayEnd;

    private static OfficeManager _officeManager;
    private static OfficeManager officeManager {
        get {
            if (_officeManager == null) {_officeManager = FindObjectOfType<OfficeManager>();}
            return _officeManager;
        }
    }

    public static void StartRandomCombat()
    {
        StartCombat(Coworkers.GetRandom().Value);
    }

    public static void StartCombat(Enemy enemy)
    {
        officeManager.gameObject.SetActive(false);
        Enemy = enemy;
        SceneManager.LoadScene("Combat", LoadSceneMode.Additive);
        OnStartCombat?.Invoke(enemy);
    }

    public static void ReturnToOffice()
    {
        SceneManager.UnloadSceneAsync("Combat");
        Enemy = null;
        officeManager.gameObject.SetActive(true);
        OnCompleteCombat?.Invoke();
    }
    public static void EndDay()
    {
        SceneManager.UnloadSceneAsync("Combat");
        Enemy = null;
        WorkDay += 1;
        officeManager.gameObject.SetActive(true);
        OnDayEnd?.Invoke();
    }
}
