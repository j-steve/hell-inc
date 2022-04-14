using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    public bool inScreen = false;
    bool gotTextForDay = false;
    bool writing = false;
    bool loadOffice = false;
    public DungeonHallMaker office;
    int textSpeedTrack;
    public int textSpeed = 5;
    int battleLinePosition;
    public TextMeshProUGUI textField;
    List<string> day1;
    List<string> day2;
    List<string> day3;
    List<string> day4;
    List<string> day5;
    //public float timeDelay = 3;
    public float timeSpent = 0;
    public int lineNumber = 0;
    List<string> text;
    List<LoadingSpriteAnimation> sprites;
    public SpriteRenderer sprite1;
    public SpriteRenderer sprite2;
    public SpriteRenderer sprite3;
    public SpriteRenderer sprite4;
    public SpriteRenderer sprite5;

    List<LoadingSpriteAnimation> day1Sprites;
    List<LoadingSpriteAnimation> day5Sprites;

    public void Initiate()
    {
        day1 = new List<string>();
        day1.Add("You are Abaddon, usually called Aba, and most commonly known to the world as Sloth, one of the deadly sins(although when has anyone ever died from relaxing and taking it easy??).");
        day1.Add("Being true to your namesake, you don't always get a lot of work done and your boss Lou(Pride) gave you one work week to shape up or you're fired. However that's just not your style. ");
        day1.Add("Instead of doing work, maybe getting your coworker sins on your side will help you out of this jam. ");
        day1.Add("^^");
        day1.Add("Talk to your coworkers and show them you are listening to them. Conversations will eventually tire you out, but you can always sleep it off and get going again the next day.^");
        day1.Add("Find items and give them to who wants them. Bonding with a coworker will eventually reveal what they like and in return you will pick up their specialties to help you not get fired.^");
        day1.Add("");

        day1Sprites = new List<LoadingSpriteAnimation>();
        day1Sprites.Add(new LoadingSpriteAnimation("Aba.png", 0, 4, 1, false));
        day1Sprites.Add(new LoadingSpriteAnimation("Lou.png", 1, 3, 2, false));
        day1Sprites.Add(new LoadingSpriteAnimation("Asmo.png", 4, 8, 1, false));
        day1Sprites.Add(new LoadingSpriteAnimation("Belphie.png", 4, 8, 2, false));
        day1Sprites.Add(new LoadingSpriteAnimation("BeezleBob.png", 4, 8, 3, false));
        day1Sprites.Add(new LoadingSpriteAnimation("Mams.png", 4, 8, 4, false));
        day1Sprites.Add(new LoadingSpriteAnimation("Sathy.png", 4, 8, 5, false));
    }

    public void StartText()
    {
        inScreen = true;
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
            if (!gotTextForDay)
            {
                textSpeedTrack = textSpeed;
                if (Utilities.day == 1)
                {
                    text = day1;
                    sprites = day1Sprites;
                }
                else if (Utilities.day == 2)
                {
                    text = day2;
                    sprites = new List<LoadingSpriteAnimation>();
                }
                else if (Utilities.day == 3)
                {
                    text = day3;
                    sprites = new List<LoadingSpriteAnimation>();
                }
                else if (Utilities.day == 4)
                {
                    text = day4;
                    sprites = new List<LoadingSpriteAnimation>();
                }
                else if (Utilities.day == 5)
                {
                    text = day5;
                    sprites = day5Sprites;
                }
                gotTextForDay = true;
                timeSpent = 0;
                lineNumber = 0;
            }
            if (gotTextForDay)
            {
                /*if (timeSpent > (lineNumber * timeDelay))
                {
                    writing = true;
                }
                else
                {
                    writing = false;
                }    

                if(writing)*/
                {
                    if (textSpeed == textSpeedTrack)
                    {
                        if (battleLinePosition + 1 <= text[lineNumber].Length)
                        {
                            if(text[lineNumber].Substring(battleLinePosition, 1) != "^")
                            {
                                textField.text += text[lineNumber].Substring(battleLinePosition, 1);
                            }
                            else
                            {
                                textField.text = (textField.text + text[lineNumber].Substring(battleLinePosition, 1)).Replace("^", "\n");
                            }

                            
                        }
                        battleLinePosition++;
                        textSpeedTrack = 0;
                    }
                    else
                    {
                        textSpeedTrack++;
                    }

                    if (battleLinePosition == text[lineNumber].Length + 1)
                    {
                        battleLinePosition = 0;
                        lineNumber++;
                    }
                }
            }
            if (lineNumber >= text.Count)
            {
                inScreen = false;
                loadOffice = true;
            }

            timeSpent += Time.deltaTime;
        }

        if (loadOffice)
        {
            loadOffice = false;
            office.gameObject.SetActive(true);
            office.StartGame();
            gameObject.SetActive(false);
        }
    }

    public class LoadingSpriteAnimation
    {
        string sprite;
        int spawnNumber;
        int leaveNumber;
        int spriteObject;
        bool placed;

        public LoadingSpriteAnimation(string sprite, int spawnNumber, int leaveNumber, int spriteObject, bool placed)
        {
            this.sprite = sprite;
            this.spawnNumber = spawnNumber;
            this.leaveNumber = leaveNumber;
            this.spriteObject = spriteObject;
            this.placed = placed;
        }

        public string Sprite { get => sprite; set => sprite = value; }
        public int SpawnNumber { get => spawnNumber; set => spawnNumber = value; }
        public int LeaveNumber { get => leaveNumber; set => leaveNumber = value; }
        public int SpriteObject { get => spriteObject; set => spriteObject = value; }
        public bool Placed { get => placed; set => placed = value; }
    }
}
