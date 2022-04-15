using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeManager : MonoBehaviour
{
    public Enemy Pride;
    public Enemy Lust;
    public Enemy Gluttony;
    public Enemy Greed;
    public Enemy Wrath;
    public Enemy Envy;
    public List<Enemy> Coworkers;
    public GameObject dungeonTileList;
    public int NumberOfItemsPerDay = 5;

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
        //StartCoroutine("DropCoworkers");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DropCoworkers()
    {
        
        foreach (Enemy coworker in Coworkers)
        {
            bool suitableDrop = false;
            print("hello from DropCoworkers");
            while (suitableDrop == false)
            {
                int randTile = Random.Range(0,dungeonTileList.transform.childCount);
                if (!dungeonTileList.transform.GetChild(randTile).GetComponent<ProtoTileBehavior>().hasItemOrEnemy)
                {
                    Vector3 dropPos = dungeonTileList.transform.GetChild(randTile).GetComponent<ProtoTileBehavior>().dropPoint;
                    coworker.gameObject.transform.position = dropPos;
                    dungeonTileList.transform.GetChild(randTile).GetComponent<ProtoTileBehavior>().hasItemOrEnemy = true;
                    suitableDrop=true;
                }
                else
                {
                    //do nothing
                }
            }
            
            
        }
    }

    public void DropItems()
    {
        List<ItemInfo> items = new List<ItemInfo>();
        for (int i = 0; i < NumberOfItemsPerDay; i++)
        {
            items.Add(Utilities.GetRandomItem());
        }

    }
}
