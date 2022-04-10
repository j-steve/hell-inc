using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WordSpawnerController : MonoBehaviour
{
    static Dictionary<char, string> CHAR_PIXELS = new Dictionary<char, string>{
    {'a',"00000010000001010100000011100000000001000000000000"},
    {'b',"01111110000000100100000010010000000110000000000000"},
    {'c',"00001110000001000100000100010000000000000000000000"},
    {'d',"00000110000000100100000010010001111110000000000000"},
    {'e',"00001110000001010100000101010000001100000000000000"},
    {'f',"00010000000111111100010100000000000000000000000000"},
    {'g',"00000110000000100101000010010100000111100000000000"},
    {'h',"01111111000000100000000010000000000111000000000000"},
    {'i',"00101111000000000000000000000000000000000000000000"},
    {'j',"00000001100000000001000101111000000000000000000000"},
    {'k',"01111111000000110000000110100000010001000000000000"},
    {'l',"01111111000000000000000000000000000000000000000000"},
    {'m',"00011111000000100000000011110000001000000000011100"},
    {'n',"00011111000000100000000010000000000111000000000000"},
    {'o',"00000110000000100100000010010000000110000000000000"},
    {'p',"00001111110000100100000010010000000110000000000000"},
    {'q',"00000110000000100100000010010000000111110000000001"},
    {'r',"00011111000000100000000010000000000100000000000000"},
    {'s',"00001001000001010100000101100000000000000000000000"},
    {'t',"00010000000111111100000100010000000000000000000000"},
    {'u',"00001111000000000100000000010000001111000000000000"},
    {'v',"00011000000000011000000000010000000110000001100000"},
    {'w',"00001110000000000100000001100000000001000000111000"},
    {'x',"00010001000000101000000001000000001010000001000100"},
    {'y',"00001000010000010010000000110000000100000000100000"},
    {'z',"00010011000001010100000110010000000000000000000000"}
    };

    const int MAX_CHAR_WIDTH_PIXELS = 5;
    const int MAX_CHAR_HEIGHT_PIXELS = 10;

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
            SpawnWord(wordList.Dequeue());
            lastCharSpawnTime = Time.time;
        }
        
    }

    void SpawnWord(string word)
    {
        WordController wordObj = Instantiate(wordPrefab, transform).Initialize();
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
                    WordPixelController pixel = Instantiate(pixelPrefab, parentWord.transform);
                    pixel.transform.localPosition =new Vector3(pixelCol + charOffset, -pixelRow, 0);
                    pixel.name = "Pixel " + character;
                    pixel.GetComponent<Renderer>().material.color = parentWord.color;
                    pixel.GetComponent<Rigidbody>().AddForce(new Vector3(-200 - 2000 * parentWord.speed, 1));
                    if (pixelCol > charWidth) { charWidth = pixelCol; }
                }
            }
        }
        return charWidth;
    } 
}
