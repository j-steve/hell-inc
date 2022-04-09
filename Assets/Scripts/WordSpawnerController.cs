using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WordSpawnerController : MonoBehaviour
{
    static Dictionary<char, string> CHAR_PIXELS = new Dictionary<char, string>{
    {'a',"0000001000010101000011100000000100000000"},
    {'b',"0011111000001001000010010000011000000000"},
    {'c',"0000011000001001000010010000000000000000"},
    {'d',"0000011000001001000010010011111000000000"},
    {'e',"0000111000010101000101010000100000000000"},
    {'f',"0000100000111111001010000000000000000000"},
    {'g',"0000100000010101000101010001111000000000"},
    {'h',"0011111100000100000001000000001100000000"},
    {'i',"0001011100000000000000000000000000000000"},
    {'j',"0000011000000001001011100000000000000000"},
    {'k',"0011111100001100000110100001000100000000"},
    {'l',"0011111100000000000000000000000000000000"},
    {'m',"0001111100001000000011110000100000000111"},
    {'n',"0001111100001000000010000000011100000000"},
    {'o',"0000011000001001000010010000011000000000"},
    {'p',"0011111100100100001001000001100000000000"},
    {'q',"0001100000100100001001000001111100000001"},
    {'r',"0001111100001000000010000000010000000000"},
    {'s',"0000100100010101000101100000000000000000"},
    {'t',"0000100000111111000010010000000000000000"},
    {'u',"0000111000000001000000010000111000000000"},
    {'v',"0001100000000110000000010000011000011000"},
    {'w',"0000111000000001000011100000000100001110"},
    {'x',"0001000100001010000001000000101000010001"},
    {'y',"0001000000001001000001100000100000010000"},
    {'z',"0001001100010101000110010000000000000000"}
    };

    const int MAX_CHAR_WIDTH_PIXELS = 5;
    const int MAX_CHAR_HEIGHT_PIXELS = 8;

    [SerializeField] WordPixelController pixelPrefab;
    [SerializeField] WordController wordPrefab;
    [SerializeField] float wordRate = 99999999999999999;
    //[SerializeField] Queue<char> wordBuffer;
    Queue<string> wordList = new Queue<string>();
    float lastCharSpawnTime = 0;

    // Start is called before the first frame update
    void OnEnable()
    {
        wordList = new Queue<string>("the quick brown fox jumps over the lazy dog This is some random words dude omg can you believe how many words are here there are like a thousand words".ToLowerInvariant().Split(" "));
        //wordBuffer = new Queue<char>(("This is some random words dude".ToLowerInvariant().Split(" ")).ToCharArray())) ;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (wordList.Count == 0) {
            return; // TODO
        }
        if ( lastCharSpawnTime + (1 * wordRate) < Time.time) {
            Debug.LogFormat("lastCharSpawnTime: {0}, time is now {1}", lastCharSpawnTime, Time.time);
            SpawnWord(wordList.Dequeue());
            lastCharSpawnTime = Time.time;
        }
        
    }

    void SpawnWord(string word)
    {
        WordController wordObj = WordController.Create(wordPrefab, transform);
        wordObj.name = "Word: ";
        int charOffset = 0;
        foreach (char nextChar in word) {
            wordObj.name += nextChar;
            if (nextChar != ' ') {
                charOffset += SpawnLetter(nextChar, charOffset, wordObj) + 2;
            }
        }
        
    }

     int SpawnLetter(char character, int charOffset, WordController parentWord)
    {
        Debug.LogFormat("Spawning letter \"{0}\"", character);
        if (!CHAR_PIXELS.ContainsKey(character)) {
            Debug.LogWarning("Failed to retrieve character " + character);
            return 0;
        }
        string charPixels = CHAR_PIXELS[character];
        int charWidth = 1;
        for (int pixelCol = 0; pixelCol < MAX_CHAR_WIDTH_PIXELS; pixelCol++) {
            for (int pixelRow = 0; pixelRow < MAX_CHAR_HEIGHT_PIXELS; pixelRow++) {
                int pixelIndex = pixelCol * MAX_CHAR_HEIGHT_PIXELS + pixelRow;
                if (charPixels[pixelIndex] == '1') {
                    Vector3 spawnPosition = new Vector3(pixelCol + charOffset + transform.position.x, -pixelRow + transform.position.y, transform.position.z);
                    WordPixelController pixel = Instantiate(pixelPrefab, spawnPosition,  Quaternion.identity, parentWord.transform);
                    pixel.name = "Pixel " + character;
                    pixel.GetComponent<Renderer>().material.color = parentWord.color;
                    if (pixelCol > charWidth) { charWidth = pixelCol; }
                }
            }
        }
        return charWidth;
    }



}
