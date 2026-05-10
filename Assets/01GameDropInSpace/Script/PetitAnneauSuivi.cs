using UnityEngine;

/// <summary>
/// A ajouter automatiquement sur chaque petit anneau.
/// Fait pointer l'anneau vers la planete cible en temps reel.
/// </summary>
public class PetitAnneauSuivi : MonoBehaviour
{
    public Transform cible; // La planete cible — assignee par GrandAnneau

    void Update()
    {
        if (cible == null) return;

        // Pointe toujours vers la planete meme si elle bouge
        transform.rotation = Quaternion.LookRotation(cible.position - transform.position);
    }
}
