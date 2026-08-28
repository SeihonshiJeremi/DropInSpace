using UnityEngine;

public class TeleportationRolling : MonoBehaviour
{
    [Header("Points de Téléportation")]
    public Transform pointTP;
    public Transform pointTPMonster;
    public Transform pointTPMonster02;

    [Header("Références Objets")]
    public GameObject Player;
    public GameObject Monster;
    public GameObject Monster02;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            rollingCube playerRolling = other.GetComponent<rollingCube>();

            if (playerRolling != null)
            {
                // ← ResetMovementState fait TOUT :
                // - Stoppe le roll en cours
                // - Remet position + rotation d'origine
                // - desacTouche = true (mouvement bloqué)
                // - timeDesacTouche = 3.0f (blocage 3 secondes)
                // - _collisionOK = false (reset collision)
                playerRolling.ResetMovementState();
                playerRolling.TriggerRespawnCooldown();
            }

            // ← Bloquer aussi les swipes
            if (swipeMovementDIS.instance != null)
            {
                swipeMovementDIS.instance.tDesacTouche();
                swipeMovementDIS.instance.ForceResetPress();
            }

            Debug.Log("[Teleport] Drop respawné — mouvement bloqué 3s");
        }

        if (other.CompareTag("Monster") || other.CompareTag("MonsterChangeColor"))
        {
            if (Monster != null && pointTPMonster != null)
                Monster.transform.position = pointTPMonster.position;
        }

        if (other.CompareTag("Monster02"))
        {
            if (Monster02 != null && pointTPMonster02 != null)
                Monster02.transform.position = pointTPMonster02.position;
        }
    }
}