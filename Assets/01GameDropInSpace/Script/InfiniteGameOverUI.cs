using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class InfiniteGameOverUI : MonoBehaviour
{
    [Header("Panel Game Over")]
    public GameObject panelGameOver;

    [Header("Textes")]
    public TextMeshProUGUI texteScore;
    public TextMeshProUGUI texteMeilleurScore;
    public TextMeshProUGUI texteVague;
    public TextMeshProUGUI texteNouveauRecord;

    [Header("Boutons")]
    public Button btnRejouer;
    public Button btnMenu;

    [Header("Noms de scènes")]
    public string nomSceneInfini = "Stage_Infini";
    public string nomSceneMenu = "MainMenu";

    void Start()
    {
        if (panelGameOver != null) panelGameOver.SetActive(false);
        if (texteNouveauRecord != null) texteNouveauRecord.gameObject.SetActive(false);
        if (btnRejouer != null) btnRejouer.onClick.AddListener(Rejouer);
        if (btnMenu != null) btnMenu.onClick.AddListener(RetourMenu);
    }

    public void AfficherGameOver(int score, int meilleurScore, int vague)
    {
        if (panelGameOver != null) panelGameOver.SetActive(true);

        if (texteScore != null) texteScore.text = FormaterTemps(score);
        if (texteMeilleurScore != null) texteMeilleurScore.text = "Meilleur : " + FormaterTemps(meilleurScore);
        if (texteVague != null) texteVague.text = "Vague atteinte : " + vague;

        bool nouveauRecord = score >= meilleurScore && score > 0;
        if (texteNouveauRecord != null) texteNouveauRecord.gameObject.SetActive(nouveauRecord);

        SelectionManager sm = Object.FindFirstObjectByType<SelectionManager>();
        if (sm != null && btnRejouer != null) sm.ActiverModeGameOver(btnRejouer.gameObject);

        StartCoroutine(AnimationEntree());
    }

    private IEnumerator AnimationEntree()
    {
        if (panelGameOver == null) yield break;
        CanvasGroup cg = panelGameOver.GetComponent<CanvasGroup>();
        if (cg == null) cg = panelGameOver.AddComponent<CanvasGroup>();
        cg.alpha = 0f;
        float t = 0f;
        while (t < 0.4f)
        {
            t += Time.unscaledDeltaTime;
            cg.alpha = Mathf.Clamp01(t / 0.4f);
            yield return null;
        }
        cg.alpha = 1f;
    }

    private string FormaterTemps(int secondes)
    {
        int min = secondes / 60;
        int sec = secondes % 60;
        return $"{min:00}:{sec:00}";
    }

    public void Rejouer()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nomSceneInfini);
    }

    public void RetourMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nomSceneMenu);
    }
}