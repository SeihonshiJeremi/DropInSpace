using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class highScoreTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    //private List<HighScoreEntry> highScoreEntryList;
    private List<Transform> highScoreEntryTransformList;

    private void Awake() 
    {
        entryContainer = transform.Find("HighScoreEntryContainer");
        entryTemplate = entryContainer.Find("HighScoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

       /* highScoreEntryList = new List<HighScoreEntry>() {
            new HighScoreEntry { score = 000000, name = "AAA"},
            new HighScoreEntry { score = 000000, name = "AAA"},
            new HighScoreEntry { score = 000000, name = "AAA"},
            new HighScoreEntry { score = 000000, name = "AAA"},
            new HighScoreEntry { score = 000000, name = "AAA"},
            new HighScoreEntry { score = 000000, name = "AAA"},
            new HighScoreEntry { score = 000000, name = "AAA"},
        };*/

        //AddHighScoreEntry(10, "JER"); /*a partir de là je peux créer une fonction qui me fera un nouveau score*/

        string jsonString = PlayerPrefs.GetString("highScoreTable");
        HighScores highScores = JsonUtility.FromJson<HighScores>(jsonString);
        

        //Sort entry list by Score
        for (int i = 0; i < highScores.highScoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < highScores.highScoreEntryList.Count; j++)
            {
                if (highScores.highScoreEntryList[j].score > highScores.highScoreEntryList[i].score)
                {
                    //swap
                    HighScoreEntry tmp = highScores.highScoreEntryList[i];
                    highScores.highScoreEntryList[i] = highScores.highScoreEntryList[j];
                    highScores.highScoreEntryList[j] = tmp;
                }
            }
        }

        highScoreEntryTransformList = new List<Transform>();
        foreach(HighScoreEntry highScoreEntry in highScores.highScoreEntryList)
        {
            CreateHighScoreEntryTransform(highScoreEntry, entryContainer, highScoreEntryTransformList);
        }
    }

private void CreateHighScoreEntryTransform(HighScoreEntry highScoreEntry, Transform container, List<Transform> transformList)
    {

        float templateHeight = 70f;
        Transform entryTransform = Instantiate(entryTemplate, container);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
            entryTransform.gameObject.SetActive(true);

            int rank = transformList.Count + 1;
            string rankString;
            switch (rank)
            {
                default:
                    rankString = rank + "TH"; break;
                case 1: rankString = "1ST"; break;
                case 2: rankString = "2ND"; break;
                case 3: rankString = "3RD"; break;
            }
            entryTransform.Find("posText").GetComponent<TMPro.TextMeshProUGUI>().text = rankString;

            int score = highScoreEntry.score;

            entryTransform.Find("scoreText").GetComponent<TMPro.TextMeshProUGUI>().text = score.ToString();

            string name = highScoreEntry.name;

            entryTransform.Find("nameText").GetComponent<TMPro.TextMeshProUGUI>().text = name;

        //set bg visible odds and evens , easier to read
       entryTransform.Find("backgroundTemplate").gameObject.SetActive(rank % 2 == 1);

        //highlight first
        if (rank == 1)
        {           
            entryTransform.Find("posText").GetComponent<TMPro.TextMeshProUGUI>().color = Color.green;
            entryTransform.Find("scoreText").GetComponent<TMPro.TextMeshProUGUI>().color = Color.green;
            entryTransform.Find("nameText").GetComponent<TMPro.TextMeshProUGUI>().color = Color.green;
        }

        if(rank >= 11) // pour desac au dessus de ....
        {
            entryTransform.gameObject.SetActive(false);
        }

        transformList.Add(entryTransform);
        
    }

    private void AddHighScoreEntry(int score, string name)
    {
        //create highscoreentry
        HighScoreEntry highScoreEntry = new HighScoreEntry { score = score, name = name };

        //load saved score
        string jsonString = PlayerPrefs.GetString("highScoreTable");
        HighScores highScores = JsonUtility.FromJson<HighScores>(jsonString);

        //add new entry to scores
        highScores.highScoreEntryList.Add(highScoreEntry);

        //save updated
        string json = JsonUtility.ToJson(highScores);
        PlayerPrefs.SetString("highScoreTable", json);
        PlayerPrefs.Save();
    }

    private class HighScores
    {
        public List<HighScoreEntry> highScoreEntryList;
    }

    /* represents a single score entry*/
    [System.Serializable]
    private class HighScoreEntry
    {
        public int score;
        public string name;
    }
}
