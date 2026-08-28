using UnityEngine;
using System.Linq;

public class ForceWakeRigidbodies : MonoBehaviour
{
    void Start()
    {
        // Wake up tous les Rigidbodies dans la scène
        var rbs = FindObjectsOfType<Rigidbody>();
        foreach (var rb in rbs)
        {
            rb.WakeUp();
            rb.useGravity = true;
            Debug.Log($"[ForceWake] {rb.gameObject.name} : gravity={rb.useGravity}, kinematic={rb.isKinematic}");
            
            // Vérification des constraints
            if (rb.constraints != RigidbodyConstraints.None)
            {
                Debug.LogWarning($"[ForceWake] {rb.gameObject.name} a des constraints : {rb.constraints}");
            }
        }
    }
}