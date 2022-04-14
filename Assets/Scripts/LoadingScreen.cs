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
    List<LoadingSpriteAnimation> day5Sprites;

    public void Initiate()
    {
        day1 = new List<string>();
        day1.Add("You are Abaddon, usually called Aba, and most commonly known to the world as Sloth, one of the deadly sins(although when has anyone ever died from relaxing and taking it easy??).^");
        day1.Add("Being true to your namesake, you don't always get a lot of work done and your boss Lou(Pride) gave you one work week to shape up or you're fired. However that's just not your style.^");
        day1.Add("Instead of doing work, maybe getting your coworker sins on your side will help you out of this jam.");
        day1.Add("^^");
        day1.Add("Talk to your coworkers and show them you are listening to them. Conversations will eventually tire you out, but you can always sleep it off and get going again the next day.^");
        day1.Add("Find items and give them to who wants them. Bonding with a coworker will eventually reveal what they like and in return you will pick up their specialties to help you not get fired.");
        day1.Add("");

        day1Sprites = new List<LoadingSpriteAnimation>();
        day1Sprites.Add(new LoadingSpriteAnimation(aba, 0, 2, 1, false));
        day1Sprites.Add(new LoadingSpriteAnimation(lou, 1, 2, 2, false));
        day1Sprites.Add(new LoadingSpriteAnimation(asmo, 4, 8, 1, false));
        day1Sprites.Add(new LoadingSpriteAnimation(belphie, 4, 8, 2, false));
        day1Sprites.Add(new LoadingSpriteAnimation(beezlebob, 4, 8, 3, false));
        day1Sprites.Add(new LoadingSpriteAnimation(mams, 4, 8, 4, false));
        day1Sprites.Add(new LoadingSpriteAnimation(sathy, 4, 8, 5, false));
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
                StartCoroutine(Wait());
            }
        }
    public IEnumerator Wait()
    {
        office.gameObject.SetActive(true);
        office.StartGame();
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
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