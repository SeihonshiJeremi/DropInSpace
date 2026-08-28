using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class collectibleManager : MonoBehaviour
{
    [SerializeField]
    private int currentPiece;
    public Text PieceText;

    void Start()
    {
        if (PlayerPrefs.HasKey("pieceMario"))
        {
            currentPiece = PlayerPrefs.GetInt("pieceMario");
            Debug.Log("score charge");
        }
    }

    void Update()
    {
        PlayerPrefs.SetInt("pieceMario", currentPiece);
        PieceText.text = " " + currentPiece;
        Debug.Log("score save");        
    }


    public void AddPiece(int pieceToAdd)
    {
        currentPiece += pieceToAdd;
    }
}
