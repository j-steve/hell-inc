using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DungeonHallMaker : MonoBehaviour
{
public int numOfTiles;

public int numOfUniqueRooms;

public GameObject tile;
public List<GameObject> tileList;
public float stepDist = 4;
public float meanderDegree = 50;
public bool isStarterHall = false;
public List<GameObject> splitRoomList;
public GameObject[] splitRoomTypes;
public int numOfTilesInSection = 0;
public Camera mainCamera;
public Canvas loadingCanvas;
public List<GameObject> FinalTileList;


    // Start is called before the first frame update
    void Start()
    {
        //Remove this later. PJM
       //StartGame();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void StartGame()
    {
        if (isStarterHall)
        {
            PlaceTile();
            StartPlacing();
            mainCamera.transform.position = this.transform.position + new Vector3(1,0,0);//Increase by 1 to align to the tiles
        }
    }

    public void DecideNextLocation()
    {
        int rand;
        rand = Random.Range(1,100);
        print(rand);
        print(transform.position);
        //Determine path movement
        if (rand <= meanderDegree)
        {
            transform.position += transform.forward * stepDist;
        }
        else
        {
            rand = Random.Range(0,100);
            if (rand < 50)
        {
            transform.Rotate(transform.up * 90);
            transform.position += transform.forward * stepDist;
        }
        if (rand >= 50)
        {
            transform.Rotate(transform.up * -90);
            transform.position += transform.forward * stepDist;
        }
        }    
        

        PlaceTile();
    }

    

    public void PlaceTile()
    {
        //Check for the presence of an already existing tile
        if (Physics.OverlapBox(transform.position,new Vector3(1.5f,1.5f,1.5f),Quaternion.identity).Length == 0)
        {
            GameObject placedTile = Instantiate(tile, transform.position, Quaternion.identity);
            numOfTilesInSection ++;
            tileList.Add(placedTile);
        }
        
    }

    public void StartPlacing()
    {
        for (int i = 0; i <= numOfTiles; i++)
        {
            DecideNextLocation();
        }
        foreach (GameObject tilePiece in tileList)
        {
            tilePiece.transform.SetParent(transform);
        }
        if (isStarterHall)
        {
            PlaceSplitRooms();
            StartCoroutine(TimedManifestTiles());
        }
        
    }

    public void ManifestTiles()
    {
        BroadcastMessage("CallSpawnTile",this);
    }

    public void PlaceSplitRooms()
    {
        for (int i = 0; i < numOfUniqueRooms; i++)
        {
            int rand = Random.Range(0, numOfTilesInSection);
            int randtype = Random.Range(0, splitRoomTypes.Length -1);
            print("Rand is: " + rand);
            print("RandType is: " + randtype);
            print(tileList.Capacity);
            GameObject newSplitRoom = Instantiate(splitRoomTypes[randtype],tileList[rand].transform.position,Quaternion.identity,transform);
            splitRoomList.Add(newSplitRoom);
            newSplitRoom.transform.Rotate(transform.up * (Random.Range(0,3) * 90));
        }
        foreach (GameObject splitRoom in splitRoomList)
        {
            splitRoom.GetComponent<DungeonHallMaker>().StartPlacing();
        }

    }

    public IEnumerator TimedManifestTiles()
    {
        yield return new WaitForSeconds(2);
        ManifestTiles();
        loadingCanvas.gameObject.SetActive(false);
    } 

    public void AddTileToFinalList(GameObject tile)
    {
        FinalTileList.Add(tile);
    }

}
