using UnityEngine;
using TMPro;

/// <summary>
/// Cache le textScore en mode Exploration.
/// A placer sur le prefabNiveau.
/// Le score continue de fonctionner en arriere-plan.
/// </summary>
public class CacherScoreExploration : MonoBehaviour
{
    [Header("UI Score a cacher en Exploration")]
    public GameObject textScore; // Assigner textScore dans l'Inspector

    void Start()
    {
        // Si on est en mode Exploration — cache le score
        if (PlayerPrefs.GetInt("ModeExploration", 0) == 1)
        {
            if (textScore != null) textScore.SetActive(false);
        }
    }
}