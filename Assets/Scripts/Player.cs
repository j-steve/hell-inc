using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int moveDistance = 10;
    public int moveSpeed = 10;
    public int rotationSpeed = 60;
    float endPosition;
    float rotation = 0.0f;
    float lastY;
    Quaternion qTo = Quaternion.identity;
    PlayerMovement movement = PlayerMovement.None;
    bool lockPlayer = false;
    List<ItemInfo> itemInventory;
    List<GossipInfo> gossipInventory;
    public CombatModifiers combatModifiers = new CombatModifiers(1,1,1,1,1,1,1,3,1,1);
    int Stamina = 100;
    int Attentiveness; //Conversation speed/size
    int Professionalism; //Mini game character speed/bullet size
    int ConflictResolution;


    public List<ItemInfo> ItemInventory { get => itemInventory; set => itemInventory = value; }
    public List<GossipInfo> GossipInventory { get => gossipInventory; set => gossipInventory = value; }
    public bool LockPlayer { get => lockPlayer; set => lockPlayer = value; }
    public PlayerMovement Movement { get => movement; set => movement = value; }
    public float EndPosition { get => endPosition; set => endPosition = value; }

    public void AddItem(ItemInfo item)
    {
        ItemInventory.Add(item);
    }
    public void RemoveItem(ItemInfo item)
    {
        ItemInventory.RemoveAt(ItemInventory.FindIndex(i => i.Name == item.Name));
    }

    public void AddGossip(GossipInfo gossip)
    {
        GossipInventory.Add(gossip);
    }
    public void RemoveGossip(GossipInfo gossip)
    {
        GossipInventory.RemoveAt(GossipInventory.FindIndex(g => g.Name == gossip.Name));
    }

    // Start is called before the first frame update
    void Start()
    {
        combatModifiers = new CombatModifiers(1, 1, 1, 1, 1, 1, 1, 3, 1, 1);
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
                    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, moveDistance))
                    {
                        if (hit.collider.tag == "Enemy")
                        {
                            //Iniatiate combat
                        }
                    }
                    else
                    {
                        Movement = PlayerMovement.Forward;
                        EndPosition = transform.position.z + moveDistance;
                        Stamina--;
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
                    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, moveDistance))
                    {
                        if (hit.collider.tag == "Enemy")
                        {
                            //Iniatiate combat
                        }
                    }
                    else
                    {
                        Movement = PlayerMovement.Back;
                        EndPosition = transform.position.z - moveDistance;
                        Stamina--;
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
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (moveSpeed * Time.deltaTime));
                if(transform.position.z >= EndPosition)
                {
                    Movement = PlayerMovement.None;
                    transform.position = new Vector3(transform.position.x, transform.position.y, EndPosition);
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
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - (moveSpeed * Time.deltaTime));
                if (transform.position.z <= EndPosition)
                {
                    Movement = PlayerMovement.None;
                    transform.position = new Vector3(transform.position.x, transform.position.y, EndPosition);
                    //LockPlayer = true;
                }
            }
        }
        lastY = transform.rotation.y;

        if(Stamina <= 0)
        {
            //Day is over or game is lost if its the final day
        }
    }

    public enum PlayerMovement
    {
        Forward = 0, Right = 1, Back = 2, Left = 3, None = 4
    }

    public CombatModifiers GetCombatModifiersForEnemy(Enemy enemy)
    {
        return CombatModifiers.CombineModifiers(combatModifiers, enemy.enemyInfo.GetCombatTrait().Modifiers);
    }
}
