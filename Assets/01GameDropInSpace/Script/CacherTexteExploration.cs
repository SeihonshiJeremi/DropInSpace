using UnityEngine;

/// <summary>
/// Cache les lettres STAGE et le numero en mode Exploration.
/// A placer sur le Canvas du prefab sceneTransition.
/// </summary>
public class CacherTexteExploration : MonoBehaviour
{
    [Header("GameObjects a cacher en Exploration")]
    public GameObject[] objetsACacher; // Assigner S, T, A, G, E, 00 dans l'Inspector

    void Start()
    {
        // Si on est en mode Exploration — cache les lettres
        if (PlayerPrefs.GetInt("ModeExploration", 0) == 1)
        {
            foreach (var obj in objetsACacher)
            {
                if (obj != null) obj.SetActive(false);
            }
        }
    }
}