using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public static int WorkDay = 1;
}
