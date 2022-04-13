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
    PlayerModifiers modifiers;
    float attentionSpanMax;
    float attentionSpanCurrent;

    public List<ItemInfo> ItemInventory { get => itemInventory; set => itemInventory = value; }
    public bool LockPlayer { get => lockPlayer; set => lockPlayer = value; }
    public PlayerMovement Movement { get => movement; set => movement = value; }
    public float EndPosition { get => endPosition; set => endPosition = value; }
    public PlayerModifiers Modifiers { get => modifiers; set => modifiers = value; }
    public float AttentionSpanMax { get => attentionSpanMax; set => attentionSpanMax = value; }
    public float AttentionSpanCurrent { get => attentionSpanCurrent; set => attentionSpanCurrent = value; }

    public void AddItem(ItemInfo item)
    {
        ItemInventory.Add(item);
    }
    public void RemoveItem(string name)
    {
        ItemInventory.RemoveAt(ItemInventory.FindIndex(i => i.Name == name));
    }

    // Start is called before the first frame update
    void Start()
    {
        Modifiers = new PlayerModifiers(1f, 1f, 1f, 1f, 1f);
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
                        //Stamina--;
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
                        //Stamina--;
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
                transform.position = new Vector3(transform.position.x + (moveSpeed * Time.deltaTime), transform.position.y, transform.position.z);
                if(transform.position.x >= EndPosition)
                {
                    Movement = PlayerMovement.None;
                    transform.position = new Vector3(EndPosition, transform.position.y, transform.position.z);
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
                transform.position = new Vector3(transform.position.x - (moveSpeed * Time.deltaTime), transform.position.y, transform.position.z);
                if (transform.position.x <= EndPosition)
                {
                    Movement = PlayerMovement.None;
                    transform.position = new Vector3(EndPosition, transform.position.y, transform.position.z);
                    //LockPlayer = true;
                }
            }
        }
        lastY = transform.rotation.y;

        
    }

    public enum PlayerMovement
    {
        Forward = 0, Right = 1, Back = 2, Left = 3, None = 4
    }
}
