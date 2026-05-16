using System.Collections;
using UnityEngine;

/// <summary>
/// Version Infini de TeleportationRolling — NE PAS modifier l'original.
/// A placer sur TP dans Stage01-01Infini uniquement.
/// Desactiver TeleportationRolling sur le meme objet.
/// </summary>
public class TeleportationRollingInfini : MonoBehaviour
{
    public GameObject Player; // PlayerGroupe

    private bool cooldownActif = false;
    private float cooldownDuree = 3f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (cooldownActif) return;
        if (InfiniteGridManager.Instance != null && InfiniteGridManager.Instance.RespawnEnCours()) return;

        StartCoroutine(RespawnAvecCooldown());
    }

    private IEnumerator RespawnAvecCooldown()
    {
        cooldownActif = true;

        // Cache Drop hors champ immediatement — sort du trigger
        if (Player != null)
        {
            Rigidbody rb = Player.GetComponentInChildren<Rigidbody>();
            rollingCube rc = Player.GetComponentInChildren<rollingCube>();
            if (rb != null) { rb.linearVelocity = Vector3.zero; rb.angularVelocity = Vector3.zero; rb.isKinematic = true; }
            if (rc != null) { rc.enabled = false; rc.desacTouche = true; }
            Player.transform.position = new Vector3(999f, 999f, 999f);
        }

        yield return new WaitForSeconds(0.1f);

        // Lance le respawn via InfiniteGridManager
        if (InfiniteGridManager.Instance != null)
            yield return StartCoroutine(InfiniteGridManager.Instance.RespawnDrop());

        yield return new WaitForSeconds(cooldownDuree);
        cooldownActif = false;
    }
}