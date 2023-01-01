using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class highScoreTableOrigine : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;

    private List<HighScoreEntry> highScoreEntryList;
    private List<Transform> highScoreEntryTransformList;

    private void Awake()
    {
        entryContainer = transform.Find("HighScoreEntryContainer");
        entryTemplate = entryContainer.Find("HighScoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        highScoreEntryList = new List<HighScoreEntry>() {
            new HighScoreEntry { score = 000000, name = "AAA" },
            new HighScoreEntry { score = 000000, name = "AAA" },
            new HighScoreEntry { score = 000000, name = "AAA" },
            new HighScoreEntry { score = 000000, name = "AAA" },
            new HighScoreEntry { score = 000000, name = "AAA" },
            new HighScoreEntry { score = 000000, name = "AAA" },
            new HighScoreEntry { score = 000000, name = "AAA" },
            new HighScoreEntry { score = 000000, name = "AAA" },

        };

        //Sort entry list by Score
        for (int i = 0; i < highScoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < highScoreEntryList.Count; j++)
            {
                if (highScoreEntryList[j].score > highScoreEntryList[i].score)
                {
                    //swap
                    HighScoreEntry tmp = highScoreEntryList[i];
                    highScoreEntryList[i] = highScoreEntryList[j];
                    highScoreEntryList[j] = tmp;
                }
            }
        }

        highScoreEntryTransformList = new List<Transform>();
        foreach (HighScoreEntry highScoreEntry in highScoreEntryList)
        {
            CreateHighScoreEntryTransform(highScoreEntry, entryContainer, highScoreEntryTransformList);
        }

        //save updated
        HighScores highScores = new HighScores { highScoreEntryList = highScoreEntryList };
        string json = JsonUtility.ToJson(highScores);
        PlayerPrefs.SetString("highScoreTable", json);
        PlayerPrefs.Save();
        Debug.Log(PlayerPrefs.GetString("highScoreTable"));
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

        transformList.Add(entryTransform);
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
