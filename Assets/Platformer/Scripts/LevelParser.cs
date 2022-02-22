using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelParser : MonoBehaviour
{
    public string filename;
    public GameObject rockPrefab;
    public GameObject brickPrefab;
    public GameObject questionBoxPrefab;
    // public GameObject coinPrefab;
    public GameObject stonePrefab;
    public Transform environmentRoot;

    // --------------------------------------------------------------------------
    void Start()
    {
        LoadLevel();
    }

    // --------------------------------------------------------------------------
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadLevel();
        }
    }

    // --------------------------------------------------------------------------
    private void LoadLevel()
    {
        string fileToParse = $"{Application.dataPath}{"/Resources/"}{filename}.txt";
        Debug.Log($"Loading level file: {fileToParse}");

        Stack<string> levelRows = new Stack<string>();

        // Get each line of text representing blocks in our level
        using (StreamReader sr = new StreamReader(fileToParse))
        {
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                levelRows.Push(line);
            }

            sr.Close();
        }

        // Go through the rows from bottom to top
        int row = 0;
        while (levelRows.Count > 0)
        {
            string currentLine = levelRows.Pop();

            int column = 0;
            char[] letters = currentLine.ToCharArray();
            foreach (var letter in letters)
            {
                // Todo - Instantiate a new GameObject that matches the type specified by letter
                // Todo - Position the new GameObject at the appropriate location by using row and column
                // Todo - Parent the new GameObject under levelRoot

                if (letter == 'x')
                {
                   var rock = Instantiate(rockPrefab);
                   rock.transform.position = new Vector3(column + 0.5f, row + 0.5f, 0f); 
                }
                else if (letter == 's')
                {
                    var stone = Instantiate(stonePrefab);
                    stone.transform.position = new Vector3(column + 0.5f, row + 0.5f, 0f);
                }
                else if (letter == 'b')
                {
                    var brick = Instantiate(brickPrefab);
                    brick.transform.position = new Vector3(column + 0.5f, row + 0.5f, 0f);
                }
                else if (letter == '?')
                {
                    var question = Instantiate(questionBoxPrefab);
                    question.transform.position = new Vector3(column + 0.5f, row + 0.5f, 0f);
                    // var coin = Instantiate(coinPrefab);
                    // coin.transform.position = new Vector3(column + 0.5f, row + 0.5f, 0f);
                }
                
                column++;
            }
            row++;
        }
    }

    // --------------------------------------------------------------------------
    private void ReloadLevel()
    {
        foreach (Transform child in environmentRoot)
        {
           Destroy(child.gameObject);
        }
        LoadLevel();
    }
}
