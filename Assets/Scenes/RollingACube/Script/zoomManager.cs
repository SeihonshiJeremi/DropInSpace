using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class zoomManager : MonoBehaviour
{
    public int valueFinLevel;
    [SerializeField] TextMeshProUGUI text_finirLevel;

    [SerializeField] TextMeshProUGUI text_timer;
    public float timerNiveau;
    private int tempsint;

    [SerializeField] TextMeshProUGUI text_score;
    public int score;

    public bool doubleScore = false;

    public int activDoubleScore;

    public float timerStartOrigine;
    public float timerStart;
    public GameObject ObjectTimerStart;

    // Start is called before the first frame update
    void Start()
    {
       // ObjectTimerStart.GetComponent<rollingCube>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //timer
        text_timer.text = " " + tempsint;
        timerNiveau -= Time.deltaTime;
        tempsint = Mathf.RoundToInt(timerNiveau);


        //decompte dalle
        text_finirLevel.text = " " + valueFinLevel;

        //score
        text_score.text = " " + score;

        if (activDoubleScore >= 1) doubleScore = true;
        else doubleScore = false;        
    }

    //valeur level

    public void finLevel(int finLevelToAdd)
    {
        valueFinLevel += finLevelToAdd;

    }

    public void reduceFinLevel(int finLevelToAdd)
    {
        valueFinLevel -= finLevelToAdd;
        if (valueFinLevel == 0)
        {
            Debug.Log("go fin level");
            Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
        }
    }

    //valeur score
    public void scoreAdd(int scoreToAdd)
    {
        score += scoreToAdd;
        if (doubleScore == true) score += scoreToAdd * 2;

    }

    public void scoreDoubleTrue(int activDoubleScoreToAdd)
    {
        activDoubleScore += activDoubleScoreToAdd;

    }

    public void scoreDoubleFalse(int activDoubleScoreToReduce)
    {
        activDoubleScore = activDoubleScore * 0;
        Debug.Log("quand je bouge sur le rouge ");

    }
}
