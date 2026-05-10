using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class highScoreTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> highScoreEntryTransformList;

    // CORRIGÉ : clé initialisée avec un nom valide
    private string scoreTemporaire = "ScoreTemporaire";

    private int newScore;

    public void Getint()
    {
        newScore = PlayerPrefs.GetInt(scoreTemporaire, 0);
        AddHighScoreEntry(newScore, "SFL");
    }

    private void Awake()
    {
        // CORRIGÉ : initialisation de la table si elle n'existe pas
        InitHighScoreTableIfEmpty();

        // Ajoute le score de la partie
        Getint();

        entryContainer = transform.Find("HighScoreEntryContainer");
        entryTemplate = entryContainer.Find("HighScoreEntryTemplate");
        entryTemplate.gameObject.SetActive(false);

        string jsonString = PlayerPrefs.GetString("highScoreTable");
        HighScores highScores = JsonUtility.FromJson<HighScores>(jsonString);

        // CORRIGÉ : vérification null après FromJson
        if (highScores == null || highScores.highScoreEntryList == null)
            highScores = new HighScores { highScoreEntryList = new List<HighScoreEntry>() };

        // CORRIGÉ : tri avec List.Sort au lieu du bubble sort
        highScores.highScoreEntryList.Sort((a, b) => b.score.CompareTo(a.score));

        highScoreEntryTransformList = new List<Transform>();
        foreach (HighScoreEntry highScoreEntry in highScores.highScoreEntryList)
        {
            CreateHighScoreEntryTransform(highScoreEntry, entryContainer, highScoreEntryTransformList);
        }
    }

    private void InitHighScoreTableIfEmpty()
    {
        if (!PlayerPrefs.HasKey("highScoreTable"))
        {
            HighScores emptyTable = new HighScores { highScoreEntryList = new List<HighScoreEntry>() };
            string json = JsonUtility.ToJson(emptyTable);
            PlayerPrefs.SetString("highScoreTable", json);
            PlayerPrefs.Save();
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
            case 1: rankString = "1ST"; break;
            case 2: rankString = "2ND"; break;
            case 3: rankString = "3RD"; break;
            default: rankString = rank + "TH"; break;
        }

        entryTransform.Find("posText").GetComponent<TMPro.TextMeshProUGUI>().text = rankString;
        entryTransform.Find("scoreText").GetComponent<TMPro.TextMeshProUGUI>().text = highScoreEntry.score.ToString();
        entryTransform.Find("nameText").GetComponent<TMPro.TextMeshProUGUI>().text = highScoreEntry.name;

        entryTransform.Find("backgroundTemplate").gameObject.SetActive(rank % 2 == 1);

        if (rank == 1)
        {
            entryTransform.Find("posText").GetComponent<TMPro.TextMeshProUGUI>().color = Color.green;
            entryTransform.Find("scoreText").GetComponent<TMPro.TextMeshProUGUI>().color = Color.green;
            entryTransform.Find("nameText").GetComponent<TMPro.TextMeshProUGUI>().color = Color.green;
        }

        if (rank >= 11)
            entryTransform.gameObject.SetActive(false);

        transformList.Add(entryTransform);
    }

    private void AddHighScoreEntry(int score, string name)
    {
        HighScoreEntry highScoreEntry = new HighScoreEntry { score = score, name = name };

        string jsonString = PlayerPrefs.GetString("highScoreTable");
        HighScores highScores = JsonUtility.FromJson<HighScores>(jsonString);

        // CORRIGÉ : vérification null
        if (highScores == null || highScores.highScoreEntryList == null)
            highScores = new HighScores { highScoreEntryList = new List<HighScoreEntry>() };

        highScores.highScoreEntryList.Add(highScoreEntry);

        string json = JsonUtility.ToJson(highScores);
        PlayerPrefs.SetString("highScoreTable", json);
        PlayerPrefs.Save();
    }

    private class HighScores
    {
        public List<HighScoreEntry> highScoreEntryList;
    }

    [System.Serializable]
    private class HighScoreEntry
    {
        public int score;
        public string name;
    }
}