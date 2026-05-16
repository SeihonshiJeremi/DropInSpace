using System.Collections;
using UnityEngine;

/// <summary>
/// Effet de spawn — scale 0 vers 1.
/// A placer sur PlayerGroupe, Monster01 et Monster02.
/// </summary>
public class SpawnScaleEffect : MonoBehaviour
{
    [Header("Parametres")]
    public float dureAnimation = 0.6f;
    public float delaiApresSpawn = 1f;

    private rollingCube playerRolling;
    private monsterZoom monsterScript;
    private Vector3 scaleOriginale;
    private bool estPlayer = false;

    void Awake()
    {
        scaleOriginale = transform.localScale;
        playerRolling = GetComponentInChildren<rollingCube>();
        monsterScript = GetComponent<monsterZoom>();
        if (monsterScript == null)
            monsterScript = GetComponentInChildren<monsterZoom>();
        estPlayer = playerRolling != null;
        Debug.Log($"[SpawnScaleEffect] {gameObject.name} — estPlayer:{estPlayer}");
    }

    public IEnumerator LancerSpawn(Vector3 positionSurCube)
    {
        // Active le GameObject
        gameObject.SetActive(true);

        // Recupere le Rigidbody
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null) rb = GetComponentInChildren<Rigidbody>();
        Debug.Log($"[SpawnScaleEffect] LancerSpawn — rb={rb != null} estPlayer={estPlayer}");

        // Bloque la physique pendant le spawn
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        // Bloque les inputs
        if (estPlayer && playerRolling != null)
            playerRolling.tDesacTouche();
        if (!estPlayer && monsterScript != null)
            monsterScript.tempsAvantDeplacement = 9999f;

        // Place juste au dessus du cube
        transform.position = new Vector3(positionSurCube.x, positionSurCube.y + 2f, positionSurCube.z);
        transform.localScale = Vector3.zero;

        // Animation scale 0 → 1
        float elapsed = 0f;
        while (elapsed < dureAnimation)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / dureAnimation);
            float scale = EaseOutBack(t);
            transform.localScale = scaleOriginale * Mathf.Max(0f, scale);
            yield return null;
        }
        transform.localScale = scaleOriginale;

        // Attend avant de donner les controles
        yield return new WaitForSeconds(delaiApresSpawn);

        // Reactive la physique
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Debloque les inputs
        if (estPlayer && playerRolling != null)
        {
            playerRolling.enabled = true;
            playerRolling.desacTouche = false;
            playerRolling.timeDesacTouche = 0f;
            Debug.Log("[SpawnScaleEffect] Drop debloque !");
        }

        if (!estPlayer && monsterScript != null)
        {
            monsterScript.tempsAvantDeplacement = monsterScript.tempsAvantDeplacementOrigine;
            Debug.Log($"[SpawnScaleEffect] {gameObject.name} debloque !");
        }
    }

    private float EaseOutBack(float t)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1f;
        return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
    }
}