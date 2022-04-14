using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    public bool inScreen = false;
    bool gotTextForDay = false;
    List<string> day1;
    List<string> day2;
    List<string> day3;
    List<string> day4;
    List<string> day5;
    public float timeDelay = 3;
    public int lineNumber = 0;
    public SpriteRenderer sprite1;
    public SpriteRenderer sprite2;
    public SpriteRenderer sprite3;
    public SpriteRenderer sprite4;
    public SpriteRenderer sprite5;

    List<LoadingSpriteAnimation> day1Sprites;

    public void Initiate()
    {
        day1 = new List<string>();
        day1.Add("You are Abaddon, usually called Aba, and most commonly known to the world as Sloth, one of the deadly sins(although when has anyone ever died from relaxing and taking it easy??). ");
        day1.Add("Being true to your namesake, you don't always get a lot of work done and your boss Lou(Pride) gave you one work week to shape up or you're fired. However that's just not your style. ");
        day1.Add("Instead of doing work, maybe getting your coworker sins on your side will help you out of this jam. ");
        day1.Add("");
        day1.Add("Talk to your coworkers and show them you are listening to them. Conversations will eventually tire you out, but you can always sleep it off and get going again the next day.");
        day1.Add("Find items and give them to who wants them. Bonding with a coworker will eventually reveal what they like and in return you will pick up their specialties to help you not get fired.");
        day1.Add("");

        day1Sprites = new List<LoadingSpriteAnimation>();
        day1Sprites.Add(new LoadingSpriteAnimation("Aba.png", 0, 8, 1));
        day1Sprites.Add(new LoadingSpriteAnimation("Lou.png", 1, 3, 2));
    }

    // Start is called before the first frame update
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        if (inScreen)
        {
            List<string> text;
            List<LoadingSpriteAnimation> sprites;
            if (!gotTextForDay)
            {
                if (Utilities.day == 1)
                {
                    text = day1;
                    sprites = day1Sprites;
                }
            }
        }
    }

    public class LoadingSpriteAnimation
    {
        string sprite;
        int spawnNumber;
        int leaveNumber;
        int spriteObject;

        public LoadingSpriteAnimation(string sprite, int spawnNumber, int leaveNumber, int spriteObject)
        {
            this.sprite = sprite;
            this.spawnNumber = spawnNumber;
            this.leaveNumber = leaveNumber;
            this.spriteObject = spriteObject;
        }

        public string Sprite { get => sprite; set => sprite = value; }
        public int SpawnNumber { get => spawnNumber; set => spawnNumber = value; }
        public int LeaveNumber { get => leaveNumber; set => leaveNumber = value; }
        public int SpriteObject { get => spriteObject; set => spriteObject = value; }
    }
}
