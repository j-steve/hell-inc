using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{ 
    public int moveDistance = 10;
    public int moveSpeed = 10;
    public int rotationSpeed = 60;
    public OfficeManager officeManager;
    float rotation = 0.0f;
    float lastY;
    float distanceMoved = 0f;
    Quaternion qTo = Quaternion.identity;  
    int randomCombatChance = -10;
    int randomCombatChangeIncrement = 0;

    public List<ItemInfo> ItemInventory { get; set; }
    public bool LockPlayer { get; set; } = true;
    public PlayerMovement Movement { get; set; }
    public Vector3 EndPosition { get; set; }
    public PlayerModifiers Modifiers { get; private set; } = new PlayerModifiers(1f, 1f, 1f, 1f, 1f);
    public float AttentionSpanMax { get; set; } = 1;
    public float AttentionSpanCurrent { get; set; } = 1;

    public void AddItem(ItemInfo item)
    {
        ItemInventory.Add(item);
    }
    public void RemoveItem(string name)
    {
        ItemInventory.RemoveAt(ItemInventory.FindIndex(i => i.Name == name));
    }

    // Update is called once per frame
    void Update()
    {
        //LockPlayer should be true if its the enemy's turn to move, in a combat, or in a mini game
        if(!LockPlayer)
        {
            if (Movement == PlayerMovement.None)
            {
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    Movement = PlayerMovement.Left;
                    rotation -= 90;
                    qTo = Quaternion.Euler(0.0f, rotation, 0.0f);
                }
                else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    RaycastHit hit;
                    
                    bool collided = Physics.Raycast(transform.position, transform.forward, out hit, moveDistance, LayerMask.GetMask("Default"));
                    //Debug.Log(hit.collider.tag);
                    if (collided && hit.collider.tag == "Enemy")
                    {
                        Debug.Log(hit.collider.name);
                        hit.collider.GetComponentInParent<Enemy>();
                        //LockPlayer = true;
                        //officeManager.InitiateCombat(hit.collider.name);
                    }
                    else
                    {
                        collided = Physics.Raycast(transform.position, transform.forward, out hit, moveDistance);
                        if (collided && hit.collider.tag != "Tile")
                        {

                        }
                        else
                        {
                            distanceMoved = 0f;
                            Movement = PlayerMovement.Forward;
                            EndPosition = transform.position + (transform.forward * moveDistance);
                        }
                    }
                }
                else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    Movement = PlayerMovement.Right;
                    rotation += 90;
                    qTo = Quaternion.Euler(0.0f, rotation, 0.0f);
                }
                else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                {
                    RaycastHit hit;

                    bool collided = Physics.Raycast(transform.position, (transform.forward * -1), out hit, moveDistance, LayerMask.GetMask("Default"));
                    //Debug.Log(hit.collider.tag);
                    if (collided && hit.collider.tag == "Enemy")
                    {
                        Debug.Log(hit.collider.name);
                        hit.collider.GetComponentInParent<Enemy>();
                        //LockPlayer = true;
                        //officeManager.InitiateCombat(hit.collider.name);
                    }
                    else
                    {
                        collided = Physics.Raycast(transform.position, (transform.forward * -1), out hit, moveDistance);
                        if (collided && hit.collider.tag != "Tile")
                        {

                        }
                        else
                        {
                            distanceMoved = 0f;
                            Movement = PlayerMovement.Back;
                            EndPosition = transform.position + ((transform.forward * -1) * moveDistance);
                        }
                    }
                }
            }
            else if (Movement == PlayerMovement.Left)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, qTo, rotationSpeed * Time.deltaTime);
                if (transform.rotation.y == lastY)
                {
                    Movement = PlayerMovement.None;
                    //LockPlayer = true;
                }
            }
            else if (Movement == PlayerMovement.Forward)
            {
                transform.position = transform.position + transform.forward * moveSpeed * Time.deltaTime;
                distanceMoved += moveSpeed * Time.deltaTime;
                //transform.position = new Vector3(transform.position.x + (moveSpeed * Time.deltaTime), transform.position.y, transform.position.z);
                if (distanceMoved >= moveDistance)
                {
                    Movement = PlayerMovement.None;
                    transform.position = EndPosition;
                    CheckForRandomBattle();
                    //LockPlayer = true;
                }
            }
            else if (Movement == PlayerMovement.Right)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, qTo, rotationSpeed * Time.deltaTime);
                if (transform.rotation.y == lastY)
                {
                    Movement = PlayerMovement.None;
                    //LockPlayer = true;
                }
            }
            else if (Movement == PlayerMovement.Back)
            {
                transform.position = transform.position + (transform.forward * -1) * moveSpeed * Time.deltaTime;
                distanceMoved += moveSpeed * Time.deltaTime;
                //transform.position = new Vector3(transform.position.x + (moveSpeed * Time.deltaTime), transform.position.y, transform.position.z);
                if (distanceMoved >= moveDistance)
                {
                    Movement = PlayerMovement.None;
                    transform.position = EndPosition;
                    CheckForRandomBattle();
                    //LockPlayer = true;
                }
            }
        }
        lastY = transform.rotation.y;
    }

    private void CheckForRandomBattle()
    {
        if (Random.Range(0, 1000) < randomCombatChance || Input.GetKey("c") && Input.GetKey("o"))
        { 
            //Enemy encounter = GameManager.Coworkers.Values.Where(e => e.sin != Sin.Pride).ToList()[Random.Range(0, GameManager.Coworkers.Count - 1)];
            //officeManager.InitiateCombat("Lou");
            //LockPlayer = true;
            randomCombatChance = -10;
            randomCombatChangeIncrement = 0;
            GameManager.StartRandomCombat();
        }
        else
        {
            randomCombatChance += ++randomCombatChangeIncrement;
        }
    }

    public enum PlayerMovement
    {
        Forward = 0, Right = 1, Back = 2, Left = 3, None = 4
    }
}
