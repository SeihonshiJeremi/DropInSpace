using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

//[RequireComponent(typeof(AudioSource))]
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

    public string scene;
    public string sceneGameOver;

    //audio
    AudioSource m_MyAudioSource;
    public AudioClip clipNoTime;
    bool chAudio = false;

    // Start is called before the first frame update
    void Start()
    {
        m_MyAudioSource = GetComponent<AudioSource>();
        // ObjectTimerStart.GetComponent<rollingCube>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //timer
        text_timer.text = " " + tempsint;
        timerNiveau -= Time.deltaTime;
        tempsint = Mathf.RoundToInt(timerNiveau);

        if(timerNiveau <= 0) SceneManager.LoadScene(sceneGameOver);
        if (timerNiveau <= 10 && chAudio == false)
        {
            changementAudio();
            chAudio = true;
        }

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
            //Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
            SceneManager.LoadScene(scene);
        }
    }

    public void addFinLevel(int finLevelToAdd)
    {
        valueFinLevel += finLevelToAdd;
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

    //audio
    public void changementAudio()
    {
        m_MyAudioSource.clip = clipNoTime;
        m_MyAudioSource.Play();
    }
}
