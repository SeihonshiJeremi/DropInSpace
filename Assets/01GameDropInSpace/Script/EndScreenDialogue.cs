using TMPro;
using UnityEngine;
using System.Collections;

public class EndScreenDialogue : MonoBehaviour
{
    public TMP_Text dialogueText;
    public string[] phrases = {
        "Mission accomplie !",
        "Mais ce n'est que le début.",
        "Drop a besoin de toi pour décoller vers le jeu complet !",
        "Like, Abonne-toi, Partage !",
        "Ne clique pas sur 'Quitter'... ",
        "Clique sur 'Suivre' !"
    };
    
    public float typingSpeed = 0.05f; // Vitesse d'écriture

    void Start()
    {
        StartCoroutine(DialogueSequence());
    }

    IEnumerator DialogueSequence()
    {
        foreach (string phrase in phrases)
        {
            dialogueText.text = ""; // Reset
            foreach (char c in phrase.ToCharArray())
            {
                dialogueText.text += c;
                yield return new WaitForSeconds(typingSpeed);
            }
            yield return new WaitForSeconds(1.5f); // Pause entre les phrases
        }
        
        // Une fois fini, on peut activer les boutons SNS si ils étaient cachés
        // gameObject.SetActive(true); 
    }
}