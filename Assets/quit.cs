using UnityEngine;

public class quit : MonoBehaviour
{
    void Update()
    {
        if (Input.GetButtonDown("Cancel")) QuitGame();
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
