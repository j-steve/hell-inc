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
    public DungeonHallMaker dungeon;

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

    public IEnumerator DropCoworkers()
    {
        print("hello from DropCoworkers");
        yield return new WaitForSeconds(4);
        foreach (Enemy coworker in Coworkers)
        {
            bool suitableDrop = false;

            while (suitableDrop == false)
            {
                int randTile = Random.Range(0,dungeon.FinalTileList.Length);
                if (dungeon.FinalTileList[randTile] != null && !dungeon.FinalTileList[randTile].GetComponent<ProtoTileBehavior>().hasItemOrEnemy)
                {
                    Vector3 dropPos = dungeon.FinalTileList[randTile].GetComponent<ProtoTileBehavior>().dropPoint;
                    coworker.gameObject.transform.position = dropPos;
                    dungeon.FinalTileList[randTile].GetComponent<ProtoTileBehavior>().hasItemOrEnemy = true;
                    suitableDrop=true;
                }
                else
                {
                    //do nothing
                }
            }
            
            
        }
    }
}
