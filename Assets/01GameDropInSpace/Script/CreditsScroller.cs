using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Crédits qui défilent sur l'écran de fin.
/// À placer sur un GameObject dans SceneTransitionEndGame.
/// </summary>
public class CreditsScroller : MonoBehaviour
{
    [Header("UI")]
    public RectTransform creditsContainer; // Le panel qui contient tout le texte
    public TMP_Text creditsText;           // Le texte des crédits

    [Header("Paramètres")]
    public float vitesseDefile = 50f;      // Pixels par seconde
    public float delaiAvantDebut = 2f;     // Secondes avant que ça commence
    public string sceneApres = "StartDropInSpace"; // Scène après les crédits

    private bool enCours = false;
    private float positionDepart;
    private float positionFin;

    [Header("Contenu des crédits")]
    [TextArea(20, 40)]
    public string contenuCredits =
@"DROP IN SPACE


Un jeu de Jeremi Seihonshi


GAME DESIGN & DÉVELOPPEMENT
Jeremi Seihonshi


GRAPHISMES & ART
Jeremi Seihonshi


MUSIQUE
PlayOnLoop.com
Licensed under Creative Commons
Attribution 4.0


ASSETS UTILISÉS
Planet Earth Free
Ultimate 10 Plus Shader


OUTILS
Unity 6
Visual Studio


INSPIRÉ DE
Zoom - Sega Megadrive (1988)


MERCI D'AVOIR JOUÉ !


Drop reviendra dans...
Shadow Station";

    void Start()
    {
        if (creditsText != null)
            creditsText.text = contenuCredits;

        // AJOUTÉ : cache le panel au départ
        creditsContainer.gameObject.SetActive(false);

        StartCoroutine(LancerCredits());
    }

    IEnumerator LancerCredits()
    {
        yield return new WaitForSeconds(delaiAvantDebut);

        // Positionne en bas AVANT d'afficher
        creditsContainer.anchoredPosition = new Vector2(0, -Screen.height);
        creditsContainer.gameObject.SetActive(true); // AJOUTÉ
        enCours = true;
    }

    void Update()
    {
        if (!enCours) return;

        // Fait défiler vers le haut
        creditsContainer.anchoredPosition += Vector2.up * vitesseDefile * Time.deltaTime;

       /* // Quand les crédits sont finis
        if (creditsContainer.anchoredPosition.y >= positionFin)
        {
            enCours = false;
            SceneManager.LoadScene(sceneApres);
        }

        // Le joueur peut passer les crédits
        if (Input.anyKeyDown)
        {
            enCours = false;
            SceneManager.LoadScene(sceneApres);
        }*/
    }
}