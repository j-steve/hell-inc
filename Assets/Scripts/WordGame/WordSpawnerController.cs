using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

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
    {'z',"00010011000001010100000110010000000000000000000000"},
    {'.',"00000001000000000000000000000000000000000000000000"},
    {'!',"01111101000000000000000000000000000000000000000000"},
    {'?',"00110000000100000000010011010000111000000000000000"},
    {',',"00000010010000000110000000000000000000000000000000"},
    {';',"00001010010000000110000000000000000000000000000000"},
    {'-',"00001000000000100000000000000000000000000000000000"},
    {'A',"00000011000011110000110010000000111100000000001100"},
    {'B',"11111111001001000100100100010001110001000000111000"},
    {'C',"01111110001000000100100000010010000001000100001000"},
    {'D',"11111111001000000100100000010001000010000011110000"},
    {'E',"11111111001001000100100100010010000001000000000000"},
    {'F',"11111111001001000000100100000010000000000000000000"},
    {'G',"01111110001000001100100000010010001001000000111000"},
    {'H',"11111111000001000000000100000011111111000000000000"},
    {'I',"10000001001111111100100000010000000000000000000000"},
    {'J',"00000110000000000100100000010011111110001000000000"},
    {'K',"11111111000001100000001111000001100110001100001100"},
    {'L',"11111111000000000100000000010000000001000000000000"},
    {'M',"11111111000111000000001111110001110000001111111100"},
    {'N',"11111111001110000000001110000000001110001111111100"},
    {'O',"01111110001000000100100000010010000001000111111000"},
    {'P',"11111111001001000000100100000001100000000000000000"},
    {'Q',"00111110000100000100010001010001000010000011110100"},
    {'R',"11111111001001000000100111000010010110000110001100"},
    {'S',"01100010001001000100100110010010001001000100011000"},
    {'T',"10000000001000000000111111110010000000001000000000"},
    {'U',"11111110000000000100000000010000000001001111111000"},
    {'V',"11100000000001110000000000110000011100001110000000"},
    {'W',"11111110000000011100001111110000000111001111111000"},
    {'X',"11000011000010010000000110000000100100001100001100"},
    {'Y',"11000000000011000000000111110000110000001100000000"},
    {'Z',"10000011001000010100100110010010100001001100000100"},
    {'"',"11100000001111000000000000000011100000001111000000"},
    {'\'',"11100000001111000000000000000000000000000000000000"},
    {'1',"01000001001111111100000000010000000000000000000000"},
    {'2',"01000011001000010100100110010001100011000000000000"},
    {'3',"01000010001000100100100010010001110110000000000000"},
    {'4',"11110000000001000000000100000011111111000000000000"},
    {'5',"11110001001001000100100110110010001110000000000000"},
    {'6',"00111110001110100100100010010000000110000000000000"},
    {'7',"10000000001000011100100110000011100000000000000000"},
    {'8',"00110110000100100100010010010000110110000000000000"},
    {'9',"01100000001001000000100100000001111111000000000000"},
    {'0',"00111110000100000100010000010000111110000000000000"}
    };

    const int MAX_CHAR_WIDTH_PIXELS = 5;
    const int MAX_CHAR_HEIGHT_PIXELS = 10;
    const float WORD_SPAWN_RATE_SECONDS = 2;

    public event Action OnWordSpawningComplete;

    [SerializeField] bool isEnemy;
    [SerializeField] WordPixelController pixelPrefab;
    [SerializeField] WordController wordPrefab;


    CombatModifiers combatModifiers;
    Queue<string> wordList = new Queue<string>(); 
    float lastCharSpawnTime = 0;
    bool hasSpawnedAllWords = false;

    public void Initialize(string conversationText, CombatModifiers combatModifiers)
    {
        // Reset the scene by clearing any existing words.
        foreach (WordController word in GetComponentsInChildren<WordController>()) {
            Destroy(word.gameObject);
        }
        hasSpawnedAllWords = false;
        // Set the new data.
        this.combatModifiers = combatModifiers;
        wordList = new Queue<string>(conversationText.Split(" "));
    }

    void FixedUpdate()
    {
        if (!isEnemy || hasSpawnedAllWords) {
            return;
        }
        if (wordList.Count == 0) {
            OnWordSpawningComplete();
            hasSpawnedAllWords = true;
        }
        if ( lastCharSpawnTime + (1 * WORD_SPAWN_RATE_SECONDS) < Time.time) {
            SpawnWord(wordList.Dequeue(), transform.position);
            lastCharSpawnTime = Time.time;
        }
        
    }

    public void SpawnWord(string word, Vector3 spawnPosition)
    {
        WordController wordObj = Instantiate(wordPrefab, transform).Initialize(combatModifiers, !isEnemy);
        wordObj.transform.position = spawnPosition;
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
                    //pixel.transform.position = spawnPosition + new Vector3(pixelCol + charOffset, -pixelRow, 0);
                    pixel.transform.localPosition = new Vector3(pixelCol + charOffset, -pixelRow, 0);
                    pixel.name = "Pixel " + character;
                    pixel.isFromPlayer = !isEnemy;
                    pixel.GetComponent<Renderer>().material.color = parentWord.color;
                    pixel.GetComponent<Rigidbody>().AddForce(new Vector3(parentWord.speed, 0, 0));
                    pixel.tag = isEnemy ? "EnemyPixel" : "PlayerPixel";
                    if (pixelCol > charWidth) { charWidth = pixelCol; }
                }
            }
        }
        return charWidth;
    } 
}
