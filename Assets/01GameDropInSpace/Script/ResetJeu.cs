using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Reset complet du jeu — appelé quand on quitte un mode spécial
/// Le code est dans Start() plutôt que Awake() pour garantir que GameStateManager est prêt.
/// </summary>
public class ResetJeu : MonoBehaviour
{
    void Awake()
    {
        // Ne rien faire ici — GameStateManager peut ne pas être initialisé
        Debug.Log("[ResetJeu] Awake appelé — le vrai reset se fera dans Start()");
    }

    void Start()
    {
        // Wait pour que GameStateManager soit prêt
        StartCoroutine(WaitAndReset());
    }

    private System.Collections.IEnumerator WaitAndReset()
    {
        // Attend une frame pour s'assurer que tous les Awake() sont finis
        yield return null;
        
        // Double-check : si GameStateManager n'existe pas, on le crée
        if (GameStateManager.Instance == null)
        {
            Debug.LogWarning("[ResetJeu] GameStateManager manquants — création d'une instance");
            GameObject gm = new GameObject("GameStateManager");
            gm.AddComponent<GameStateManager>();
            DontDestroyOnLoad(gm);
        }
        
        // Maintenant on peut faire le reset proprement
        GameStateManager.Instance.ResetSession();
        Debug.Log("[ResetJeu] Session réinitialisée dans Start()");
    }
}