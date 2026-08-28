using UnityEngine;
using UnityEngine.SceneManagement;

public class lockMouse : MonoBehaviour
{
    [SerializeField]
    private GameObject mouseLock;

    void Awake()
    {
        if (mouseLock != null)
        {
            // CORRIGÉ : détache du parent pour que DontDestroyOnLoad fonctionne
            mouseLock.transform.SetParent(null);
            DontDestroyOnLoad(mouseLock);
        }
    }

    void Update()
    {
        // CORRIGÉ : verrouille seulement pendant les stages, pas dans les menus
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName.Contains("Stage") || sceneName.Contains("SceneTransition"))
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;
    }
}