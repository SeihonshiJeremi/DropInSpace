using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerPrefs : MonoBehaviour
{
    [SerializeField]
    private int score;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey("Score"))
        {
            score = PlayerPrefs.GetInt("Score");
            Debug.Log("score charge");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            score++;
        }

        if(Input.GetKeyDown(KeyCode.S))
        {
            PlayerPrefs.SetInt("Score", score);
                Debug.Log("score save");
        }
    }
}
