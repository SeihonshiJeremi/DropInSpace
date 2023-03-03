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
    public int scoreSetIn;

    public bool doubleScore = false;

    public int activDoubleScore;

    public float timerStartOrigine;
    public float timerStart;
    public GameObject ObjectTimerStart;

    public string scene;
    public string sceneGameOver;

    //audio
    AudioSource m_MyAudioSource;
    public AudioSource m_MyAudioSourceBonbon;
    public AudioClip audioStage;
    public AudioClip clipNoTime;
    public bool chAudio = false;

    //pour audio power
    public bool itemAudio = false;
    public bool itemAudioBonbon = false;
    public float durationItem;
    public float durationItemOrigine;
    public AudioClip ItemTimeDiv2;
    public AudioClip ItemStop;
    public AudioClip ItemInvincibility;
    public AudioClip ItemBonbon;

    //pour score
    string scoreFinLevel;
    int newScore;
    //public GameObject pourTableauDesScores;


    // Start is called before the first frame update
    void Start()
    {
        m_MyAudioSource = GetComponent<AudioSource>();
        // ObjectTimerStart.GetComponent<rollingCube>().enabled = false;

        GameObject.Find("LifeBar").GetComponent<LifeBarManager>().activationLifeBar();
    }

    /*void Getint()
    {
        
        newScore = PlayerPrefs.GetInt(scoreFinLevel);
        
        
    }*/

    // Update is called once per frame
    void Update()
    {
        //ici que je fais appel au script, la fonction des tableaux des scores        
        PlayerPrefs.SetInt(scoreFinLevel, score);
        /*Getint();
        Debug.Log("The key " + newScore + " exists");*/

        //timer item
        if (itemAudio == true)
        {
            durationItem -= Time.deltaTime;
          
            if (durationItem < 0)
            {
                audioOrigine();
                itemAudio = false;
                if(chAudio == true)
                {
                    changementAudio();
                    //chAudio = true;
                }
            }
        }

        //audio bonbon
        if(itemAudioBonbon == true)
        {
            durationItem -= Time.deltaTime;
            if (durationItem < 0)
            {
                m_MyAudioSourceBonbon.Stop();
            }
        }


        //timer
        text_timer.text = " " + tempsint;
        timerNiveau -= Time.deltaTime;
        tempsint = Mathf.RoundToInt(timerNiveau);

        if(timerNiveau <= 0) SceneManager.LoadScene(sceneGameOver);
        if (timerNiveau <= 20 && chAudio == false)
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
            //pourTableauDesScores.GetComponent<highScoreTable>().Getint();

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

    public void ajoutScoreBonbonRouge()
    {
        score = score + 200;
        Debug.Log("score + 200");
    }

    public void ajoutScoreBonbonVert()
    {
        score = score + 100;
    }

    //audio
    public void audioOrigine()
    {
        m_MyAudioSource.clip = audioStage;
        m_MyAudioSource.Play();
    }

    public void changementAudio()
    {
        m_MyAudioSource.clip = clipNoTime;
        m_MyAudioSource.Play();
    }

    public void audioItemTimeDiv2()
    {
        m_MyAudioSource.clip = ItemTimeDiv2;
        m_MyAudioSource.Play();
        itemAudio = true;
        durationItem = 5.0f;
    }

    public void audioItemTStop()
    {
        m_MyAudioSource.clip = ItemStop;
        m_MyAudioSource.Play();
        itemAudio = true;
        durationItem = 10.0f;
    }

    public void audioItemTInvincibility()
    {
        m_MyAudioSource.clip = ItemInvincibility;
        m_MyAudioSource.Play();
        itemAudio = true;
        durationItem = 10.0f;
    }

    public void audioItemTBonbon()
    {
        m_MyAudioSourceBonbon.clip = ItemBonbon;
        m_MyAudioSourceBonbon.Play();
        itemAudioBonbon = true;
        durationItem = 0.1f;
    }
}
