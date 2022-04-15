using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    List<string> day6;
    List<string> day7;
    //public float timeDelay = 3;
    public float timeSpent = 0;
    public int lineNumber = 0;
    List<string> text;
    List<LoadingSpriteAnimation> sprites;
    public Image sprite1;
    public Image sprite2;
    public Image sprite3;
    public Image sprite4;
    public Image sprite5;

    public Sprite aba;
    public Sprite lou;
    public Sprite asmo;
    public Sprite belphie;
    public Sprite beezlebob;
    public Sprite mams;
    public Sprite sathy;

    List<LoadingSpriteAnimation> day1Sprites;
    List<LoadingSpriteAnimation> day2Sprites;
    List<LoadingSpriteAnimation> day3Sprites;
    List<LoadingSpriteAnimation> day4Sprites;
    List<LoadingSpriteAnimation> day5Sprites;
    List<LoadingSpriteAnimation> day6Sprites;
    List<LoadingSpriteAnimation> day7Sprites;

    public void Initiate()
    {
        day1 = new List<string>();
        day1.Add("You are Abaddon, usually called Aba, and most commonly known to the world as Sloth, one of the seven deadly sins (although when has anyone ever died from relaxing and taking it easy??).^");
        day1.Add("True to your moniker, you don't always get a lot of work done and your boss Lou (aka Pride) gave you one work week to shape up or you're fired. However, that's just not your style.^");
        day1.Add("Instead of doing work, maybe getting your sinful coworkers on your side will help you out of this jam.");
        day1.Add("^^");
        day1.Add("Talk to your coworkers and show them you are listening to them. Conversations will eventually tire you out, but you can always sleep it off and get going again the next day.^");
        day1.Add("Find items and give them to the coworkers who want them. Bond with your coworkers to reveal what they like and, in return, their specialties will rub off on you and help you keep your job.^^");
        day1.Add("Press enter to play.");

        day2 = new List<string>();
        day2.Add("Day 2^^"); 
        day2.Add("Press enter to continue.");

        day3 = new List<string>();
        day3.Add("Day 3^^");
        day3.Add("Press enter to continue.");

        day4 = new List<string>();
        day4.Add("Day 4^^");
        day4.Add("Press enter to continue.");

        day7 = new List<string>();
        day7.Add("Patrick McLean - Artwork, Dungeon Maker, Coding^"); 
        day7.Add("Stephen Sichina - Mini Game, Coding^");
        day7.Add("Michael Thomas - Coding, Writing^");
        day7.Add("Music produced by J Apollo Productions^");
        day7.Add("Sarah Thomas - Writing, Editing^");
        day7.Add("^Thanks for playing!");

        day1Sprites = new List<LoadingSpriteAnimation>();
        day1Sprites.Add(new LoadingSpriteAnimation(aba, 0, 2, 1, false));
        day1Sprites.Add(new LoadingSpriteAnimation(lou, 1, 2, 2, false));
        day1Sprites.Add(new LoadingSpriteAnimation(asmo, 4, 8, 1, false));
        day1Sprites.Add(new LoadingSpriteAnimation(belphie, 4, 8, 2, false));
        day1Sprites.Add(new LoadingSpriteAnimation(beezlebob, 4, 8, 3, false));
        day1Sprites.Add(new LoadingSpriteAnimation(mams, 4, 8, 4, false));
        day1Sprites.Add(new LoadingSpriteAnimation(sathy, 4, 8, 5, false));

        day2Sprites = new List<LoadingSpriteAnimation>();
        day2Sprites.Add(new LoadingSpriteAnimation(aba, 0, 2, 2, false));

        day3Sprites = new List<LoadingSpriteAnimation>();
        day3Sprites.Add(new LoadingSpriteAnimation(aba, 0, 2, 3, false));

        day4Sprites = new List<LoadingSpriteAnimation>();
        day4Sprites.Add(new LoadingSpriteAnimation(aba, 0, 2, 4, false));

        day7Sprites = new List<LoadingSpriteAnimation>();
        day7Sprites.Add(new LoadingSpriteAnimation(aba, 0, 8, 1, false));
        day7Sprites.Add(new LoadingSpriteAnimation(beezlebob, 0, 8, 2, false));
        day7Sprites.Add(new LoadingSpriteAnimation(belphie, 0, 8, 3, false));
        day7Sprites.Add(new LoadingSpriteAnimation(mams, 0, 8, 4, false));
        day7Sprites.Add(new LoadingSpriteAnimation(lou, 0, 8, 5, false));
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
            if (Input.GetKey(KeyCode.Return))
            {
                inScreen = false;
                loadOffice = false;
                StartCoroutine(LoadOffice());
            }
            
            if(!gotTextForDay)
            { StartNewDay(); }

            if (gotTextForDay && !loadOffice)
            {
                foreach (LoadingSpriteAnimation l in sprites.Where(s => s.Placed))
                {
                    if (l.LeaveNumber == lineNumber)
                    {
                        switch (l.SpriteObject)
                        {
                            case 1:
                                StartCoroutine(FadeOut(sprite1));
                                break;
                            case 2:
                                StartCoroutine(FadeOut(sprite2));
                                break;
                            case 3:
                                StartCoroutine(FadeOut(sprite3));
                                break;
                            case 4:
                                StartCoroutine(FadeOut(sprite4));
                                break;
                            case 5:
                                StartCoroutine(FadeOut(sprite5));
                                break;
                        }
                        l.Placed = false;
                    }
                }
                foreach (LoadingSpriteAnimation l in sprites.Where(s => !s.Placed))
                {
                    if (l.SpawnNumber == lineNumber)
                    {
                        switch (l.SpriteObject)
                        {
                            case 1:
                                StartCoroutine(FadeIn(sprite1, l.Sprite));
                                break;
                            case 2:
                                StartCoroutine(FadeIn(sprite2, l.Sprite));
                                break;
                            case 3:
                                StartCoroutine(FadeIn(sprite3, l.Sprite));
                                break;
                            case 4:
                                StartCoroutine(FadeIn(sprite4, l.Sprite));
                                break;
                            case 5:
                                StartCoroutine(FadeIn(sprite5, l.Sprite));
                                break;
                        }
                        l.Placed = true;
                    }
                }
                    
                if (textSpeed == textSpeedTrack)
                {
                    if (battleLinePosition + 1 <= text[lineNumber].Length)
                    {
                        if (text[lineNumber].Substring(battleLinePosition, 1) != "^")
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

                if (lineNumber < text.Count && battleLinePosition == text[lineNumber].Length + 1)
                {
                    battleLinePosition = 0;
                    lineNumber++;
                }

                if (lineNumber >= text.Count)
                {
                    loadOffice = true;
                }
            }
        }

        timeSpent += Time.deltaTime;

        /*if (loadOffice)
        {
            loadOffice = false;
            StartCoroutine(LoadOffice());
        }*/
    }
    public void Day5Setup()
    {
        day5 = new List<string>();
        day5Sprites = new List<LoadingSpriteAnimation>();

        day5.Add("The day has finally come and its time to see if your slacking has paid off.^");
        day5.Add("You head towards Lou's office ready for a boss battle.^");
        int numberOfLines = 2;

        foreach (Enemy e in GameManager.Coworkers.Values)
        {
            if (e.relationshipPoints >= 500)
            {
                switch(e.sin)
                {
                    case Sin.Envy:
                        day5.Add("Yo, I'm not saying I'm jealous of you or anything but I really liked what you were doing this past week.^");
                        day5.Add("You feel like you can focus long hours, staring at every word someone says.^");
                        day5Sprites.Add(new LoadingSpriteAnimation(beezlebob, numberOfLines, numberOfLines + 1, 2, false));
                        numberOfLines += 2;
                        break;
                    case Sin.Gluttony:
                        day5.Add("Hey, I really enjoyed getting to know you better this week. When I was talking to you I almost didn't feel hungry.^");
                        day5.Add("You feel like your words are big, full, and ready to destroy.^");
                        day5Sprites.Add(new LoadingSpriteAnimation(belphie, numberOfLines, numberOfLines + 1, 2, false));
                        numberOfLines += 2;
                        break;
                    case Sin.Greed:
                        day5.Add("Howdy there, I just want you to know that if I ever go and start my own company, I'm Jerry McGuireing you.^");
                        day5.Add("You feel like you can talk so fast that no body has can get a word in.^");
                        day5Sprites.Add(new LoadingSpriteAnimation(mams, numberOfLines, numberOfLines + 1, 2, false));
                        numberOfLines += 2;
                        break;
                    case Sin.Lust:
                        day5.Add("I couldn't keep my eyes off you this week. No matter how your meeting goes, know I'd love you take you out tonight.^");
                        day5.Add("You feel like you can move with confidence and strut your stuff faster than ever before.^");
                        day5Sprites.Add(new LoadingSpriteAnimation(asmo, numberOfLines, numberOfLines + 1, 2, false));
                        numberOfLines += 2;
                        break;
                    case Sin.Wrath:
                        day5.Add("Finally someone I can rage with!! You and me are going to a McDonalds this weekend and yelling at everyone who is taking too long to order!^");
                        day5.Add("You feel like you really know how to say the most damaging thing to a person at a given time.^");
                        day5Sprites.Add(new LoadingSpriteAnimation(sathy, numberOfLines, numberOfLines + 1, 2, false));
                        numberOfLines += 2;
                        break;
                }
            }
        }

        day5.Add("^You get to Lou's door and go in.^");
        numberOfLines++;
        day5Sprites.Add(new LoadingSpriteAnimation(lou, numberOfLines, numberOfLines + 2, 2, false));
        day5Sprites.Add(new LoadingSpriteAnimation(aba, 0, numberOfLines + 2, 1, false));
        day5.Add("Alright, lets get this over with so I can turn your cubicle into my suspender storage rack.^");
        day5.Add("The final battle starts now.^");
        day5.Add("Press enter to continue.");
    }
    public void Day6Setup()
    {
        day6 = new List<string>();
        day6Sprites = new List<LoadingSpriteAnimation>();

        if (GameManager.GameWon)
        {
            day6.Add("By the end of it Lou is huffing and wheezing on the ground.^");
            day6.Add("Lou can't even stand up.^");
            day6.Add("He looks at you astonished and tells you that he will see you next week.^");
            day6.Add("You've secured sitting in your office chair for 40 hours a week doing nothing for at least another year.^");
            day6.Add("Press enter to continue.");
            day6Sprites.Add(new LoadingSpriteAnimation(aba, 0, 6, 1, false));
        }
        else
        {
            day6.Add("You just couldn't make it...^");
            day6.Add("Security has to drag you out of Lou's office as you can't even stand up.^");
            day6.Add("They throw your stuff in a box then they throw you right out the door.^");
            day6.Add("You spend the rest of your life sitting on your couch at home and staying true to your sin.^");
            day6.Add("Press enter to continue.");
            day6Sprites.Add(new LoadingSpriteAnimation(aba, 0, 6, 1, false));
        }
    }
    public void StartNewDay()
    {
        inScreen = true;
        textSpeedTrack = textSpeed;
        textField.text = "";

        Color tmp = sprite1.color;
        tmp.a = 0;
        sprite1.color = tmp;
        sprite2.color = tmp;
        sprite3.color = tmp;
        sprite4.color = tmp;
        sprite5.color = tmp;
        battleLinePosition = 0;

        if (GameManager.WorkDay == 1)
        {
            text = day1;
            sprites = day1Sprites;
        }
        else if (GameManager.WorkDay == 2)
        {
            text = day2;
            sprites = day2Sprites;
        }
        else if (GameManager.WorkDay == 3)
        {
            text = day3;
            sprites = day3Sprites;
        }
        else if (GameManager.WorkDay == 4)
        {
            text = day4;
            sprites = day4Sprites;
        }
        else if (GameManager.WorkDay == 5)
        {
            Day5Setup();
            text = day5;
            sprites = day5Sprites;
        }
        else if (GameManager.WorkDay == 6)
        {
            Day6Setup();
            text = day6;
            sprites = day6Sprites;
        }
        else if (GameManager.WorkDay == 6)
        {
            text = day7;
            sprites = day7Sprites;
        }
        gotTextForDay = true;
        timeSpent = 0;
        lineNumber = 0;
    }

    public IEnumerator LoadOffice()
    {
       GameManager.SetLoadingScreen();
        //GameManager.loadingScreen.gameObject = gameObject;
        if (GameManager.WorkDay < 5)
        {
            GameManager.officeManager.gameObject.SetActive(true);
            office.gameObject.SetActive(true);
            office.StartGame();
            yield return new WaitForSeconds(2);
            gameObject.SetActive(false);
            inScreen = false;
            gotTextForDay = false;
        }
        else if(GameManager.WorkDay == 6)
        {
            GameManager.StartCombat(GameManager.Coworkers.Values.Where(e => e.name == "Lou").Single());
        }
        else
        {
            textField.text = "";
            GameManager.StartCredits();
        }
    }

    private IEnumerator FadeIn(Image i, Sprite s)
    {
        float alphaVal = i.color.a;
        Color tmp = i.color;
        i.sprite = s;
        while (i.color.a < 1)
        {
            alphaVal += 0.01f;
            tmp.a = alphaVal;
            i.color = tmp;

            yield return new WaitForSeconds((textSpeed * 1f) / 400); // update interval
        }
    }

    private IEnumerator FadeOut(Image i)
    {
        float alphaVal = i.color.a;
        Color tmp = i.color;
        
        while (i.color.a > 0)
        {
            alphaVal -= 0.01f;
            tmp.a = alphaVal;
            i.color = tmp;

            yield return new WaitForSeconds((textSpeed * 1f) / 400); // update interval
        }
        i.sprite = null;
    }
}

public class LoadingSpriteAnimation
{
    Sprite sprite;
    int spawnNumber;
    int leaveNumber;
    int spriteObject;
    bool placed;

    public LoadingSpriteAnimation(Sprite sprite, int spawnNumber, int leaveNumber, int spriteObject, bool placed)
    {
        this.sprite = sprite;
        this.spawnNumber = spawnNumber;
        this.leaveNumber = leaveNumber;
        this.spriteObject = spriteObject;
        this.placed = placed;
    }

    public Sprite Sprite { get => sprite; set => sprite = value; }
    public int SpawnNumber { get => spawnNumber; set => spawnNumber = value; }
    public int LeaveNumber { get => leaveNumber; set => leaveNumber = value; }
    public int SpriteObject { get => spriteObject; set => spriteObject = value; }
    public bool Placed { get => placed; set => placed = value; }
}