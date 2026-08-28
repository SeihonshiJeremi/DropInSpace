using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class pauseMenuDIP : MonoBehaviour
{
    public bool GameIsPaused;
    public GameObject pauseMenuObject;
    public bool okPause = false;
    public float timer;
    public GameObject EventSystem;
    public AudioSource Audio;
    public Animator anim;

    public enum TypeScene { Arcade, Exploration, SpaceHub, Menu }
    [Header("Bouton quitter")]
    public TypeScene typeScene = TypeScene.Arcade;
    public string nomSceneMenu = "StartDropInSpace";
    public string nomSceneHub = "SpaceHub";

    private DIS_Controls controls;

    void Awake()
    {
        controls = new DIS_Controls();
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
        controls.Gameplay.Pause.performed += ctx => TogglePause();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    void Start()
    {
        timer = 2.0f;
        EventSystem.SetActive(false);
        GameIsPaused = false;
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            EventSystem.SetActive(false);
            okPause = false;
        }
        else
        {
            EventSystem.SetActive(true);
            okPause = true;
        }
    }

    private void TogglePause()
    {
        if (!okPause) return;
        if (GameIsPaused) Resume();
        else Pause();
    }

    public void Pause()
    {
        if (!okPause) return;
        Time.timeScale = 0f;
        GameIsPaused = true;
        pauseMenuObject.SetActive(true);
        Audio.Pause();
        anim.Play("pauseMenuDIPAnim");
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        pauseMenuObject.SetActive(false);
        Audio.Play();
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;

        // Nettoyage via GameStateManager
        GameStateManager.Instance.ClearAllInstances();
        GameStateManager.Instance.ResetSession();

        string sceneActuelle = SceneManager.GetActiveScene().name;

        if (sceneActuelle == "SpaceHub")
            SceneManager.LoadScene(nomSceneMenu);
        else if (GameStateManager.Instance.IsExplorationMode)
            SceneManager.LoadScene(nomSceneHub);
        else
            SceneManager.LoadScene(nomSceneMenu);
    }
}