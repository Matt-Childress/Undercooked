using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    //store instance variable for access to save/load methods
    public static SaveManager instance;

    //hold the loaded/manipulated saveData in memory
    public SaveData saveData;

    //save file name
    private const string saveFileName = "undercookedSave.json";

    private void Awake()
    {
        //initialize the static instance on awake
        instance = this;

        LoadScores(); //try to load scores
        if (saveData == null) //if this is the first time the game was opened, create new save data for the user
        {
            saveData = new SaveData();
        }
    }

    private void LoadScores()
    {
        //loading from platform's persistent data path
        string path = Path.Combine(Application.persistentDataPath, saveFileName);

        if (File.Exists(path))
        {
            try
            {
                //read save data from JSON formatted file
                string dataAsJson = File.ReadAllText(path);
                SaveData loadedData = JsonUtility.FromJson<SaveData>(dataAsJson);
                SaveManager.instance.saveData = loadedData; //saveData in memory
            }
            catch (Exception e)
            {
                //catching any I/O or JSON issues and log them
                Debug.LogWarning(e.ToString());
            }
        }
    }

    public void CheckNewScores(int score1, int score2)
    {
        //game manager passes in the 2 player scores to see if they should be included in high scores and a save performed

        if(score1 > 0 || score2 > 0) // do nothing if both scores are zero
        {
            //get the current lowest High Score (or zero if there is still room on the list)
            int lowestScore = saveData.highScores[saveData.highScores.Length - 1];

            //copy high scores to a generic list
            List<int> scoresPlusNew = saveData.highScores.ToList<int>();

            if (score1 > lowestScore) //add player 1's score to the list if it qualifies as a new high score
            {
                scoresPlusNew.Add(score1);
            }

            if (score2 > lowestScore) //add player 1's score to the list if it qualifies as a new high score
            {
                scoresPlusNew.Add(score2);
            }

            if(scoresPlusNew.Count > saveData.highScores.Length) //at least one new score was added
            {
                scoresPlusNew.Sort(); //sort the new appended list ascending
                scoresPlusNew.Reverse(); //switch to descending for high scores
                scoresPlusNew.RemoveRange(10, scoresPlusNew.Count - saveData.highScores.Length); //pop out the lowest 2 scores

                saveData.highScores = scoresPlusNew.ToArray(); //set the updated score list to the actual high scores list in save data

                SaveScores(); //perform a file save
            }
        }
    }

    private void SaveScores()
    {
        //saving to platform's persistent data path
        string path = Path.Combine(Application.persistentDataPath, saveFileName);

        try
        {
            //saveData to JSON object and write it to save file
            string dataAsJson = JsonUtility.ToJson(SaveManager.instance.saveData);
            File.WriteAllText(path, dataAsJson);
        }
        catch (Exception e)
        {
            //catch any I/O or JSON issues and log them
            Debug.LogWarning(e.ToString());
        }
    }
}
