using System.Collections;
using UnityEngine;
using TMPro;

public class ZenHaiku : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI textePrincipal;
    public TextMeshProUGUI texteSecondaire;
    public TextMeshProUGUI texteType;

    [Header("Timing")]
    public float delaiApparition = 2f;
    public float dureeVisible = 3.5f;
    public float dureeFonduIn = 1f;
    public float dureeFonduOut = 1f;

    [Header("Style")]
    public Color couleurHaiku = new Color(1f, 0.98f, 0.85f, 1f);
    public Color couleurCitation = new Color(0.85f, 0.95f, 1f, 1f);

    private static readonly string[] haikusFR = new string[]
    {
        "Une goutte tombe\ndans l'infini du cosmos\net tout recommence",
        "Chaque cube coloré\nest une étoile allumée\ndans ton propre ciel",
        "Le vide entre deux astres\nn'est pas le néant\nc'est le chemin",
        "Tu tombes, tu roules\nla planète tourne sous toi\nl'univers sourit",
        "Une goutte d'eau\nreflète toutes les étoiles\ntu es l'une d'elles",
        "Le silence spatial\nn'est pas absence de son\nmais plénitude",
        "Avancer sans peur\nmême quand le cube est rouge\nc'est ça, voyager",
        "La station flotte\nentre deux bras de la galaxie\ncomme un rêve tenu",
        "Chaque planète\ngarde un secret dans son cœur\nattends qu'elle parle",
        "La couleur monte\ndu cube vers l'atmosphère\nle voyage commence",
        "Tu n'es pas perdu\ntu explores simplement\nl'espace entre toi",
        "Une lumière lointaine\na quitté son étoile morte\nelle brille pour toi",
        "Le cosmos respire\nà un rythme que tu sens\nquand tu te tais",
        "Tomber dans l'espace\nn'est pas chuter — c'est voler\nsans le savoir encore",
        "La lumière voyage\nmille ans avant d'être vue\ntu l'as vue ce soir",
        "La dernière planète\nn'est jamais la dernière\nil y a toujours plus loin"
    };

    private static readonly string[] haikuEN = new string[]
    {
        "A drop falls softly\ninto the endless cosmos\nall begins again",
        "Each colored cube lit\nis a star you've just ignited\nin your own night sky",
        "The void between stars\nis not emptiness at all\nit is the pathway",
        "You fall, you roll on\nthe planet turns beneath you\nthe universe smiles",
        "A single water drop\nreflects every star above\nyou are one of them",
        "The silence of space\nis not the absence of sound\nbut pure wholeness",
        "Move without fear now\neven when the cube is red\nthat is journeying",
        "The station floats still\nbetween two arms of the stars\nlike a held dream",
        "Every planet holds\na secret deep in its core\nwait, it will speak soon",
        "Color rises up\nfrom the cube to atmosphere\nthe journey begins",
        "You are not lost here\nyou are simply exploring\nthe space within you",
        "A distant light left\nits already dead star\nit shines just for you",
        "The cosmos breathes slow\nat a rhythm you can feel\nwhen you grow quiet",
        "Falling through space now\nis not falling — it is flight\nyou just don't know yet",
        "Light travels so far\na thousand years before seen\nyou saw it tonight",
        "The last planet seen\nis never truly the last\nthere is always more"
    };

    private static readonly string[] citationsFR = new string[]
    {
        "« L'espace n'est pas vide.\nIl est plein de toi. »",
        "« Chaque voyage commence\npar une chute volontaire. »",
        "« La lumière ne se presse pas —\net pourtant, elle arrive toujours. »",
        "« Tu n'explores pas l'espace.\nTu t'explores toi-même à travers lui. »",
        "« Une planète sans visiteur\nest un poème sans lecteur. »",
        "« Le cube rouge n'est pas un obstacle.\nC'est une invitation à choisir autrement. »",
        "« Voyager, c'est accepter\nde ne pas savoir où l'on atterrit. »",
        "« La station tourne. Les étoiles restent.\nToi, tu choisis. »",
        "« Il faut beaucoup de silence\npour entendre l'univers. »",
        "« Une goutte dans l'espace reste une goutte.\nEt c'est suffisant. »",
        "« La gravité est une règle.\nLe voyage est un choix. »",
        "« Chaque étoile que tu vois\nest un soleil pour quelqu'un d'autre. »",
        "« Le chemin optimal n'est pas toujours le plus court —\nc'est celui que tu choisis. »",
        "« L'infini ne fait pas peur\nà celui qui avance un cube à la fois. »",
        "« Tu arrives toujours quelque part.\nMême dans le vide. »",
        "« La lumière de cette planète\nt'attendait depuis le début. »"
    };

    private static readonly string[] citationsEN = new string[]
    {
        "« Space is not empty.\nIt is full of you. »",
        "« Every journey begins\nwith a willful fall. »",
        "« Light does not hurry —\nand yet, it always arrives. »",
        "« You don't explore space.\nYou explore yourself through it. »",
        "« A planet without a visitor\nis a poem without a reader. »",
        "« The red cube is not an obstacle.\nIt is an invitation to choose differently. »",
        "« To travel is to accept\nnot knowing where you land. »",
        "« The station turns. The stars remain.\nYou choose. »",
        "« It takes a great deal of silence\nto hear the universe. »",
        "« A drop in space is still a drop.\nAnd that is enough. »",
        "« Gravity is a rule.\nTravel is a choice. »",
        "« Every star you see\nis someone else's sun. »",
        "« The optimal path is not always the shortest —\nit is the one you choose. »",
        "« Infinity does not frighten those\nwho move one cube at a time. »",
        "« You always arrive somewhere.\nEven in the void. »",
        "« The light of this planet\nhas been waiting for you all along. »"
    };

    void Awake()
    {
        // faire en sorte que les textes soient invisibles au départ et apparaissent avec un fondu
            SetAlpha(textePrincipal, 0f);
            SetAlpha(texteSecondaire, 0f);
            SetAlpha(texteType, 0f);
    }

        void Start()
    {
        if (PlayerPrefs.GetInt("ModeZenActif", 0) != 1)
        {
            gameObject.SetActive(false);
            return;
        }

        StartCoroutine(AfficherTexte());
    }

    IEnumerator AfficherTexte()
    {
        // Attend une frame que ModeZenManager finisse son OnSceneLoaded
        yield return null;

        SetAlpha(textePrincipal, 0f);
        SetAlpha(texteSecondaire, 0f);
        SetAlpha(texteType, 0f);

        yield return new WaitForSeconds(delaiApparition);

        int stageIndex = ModeZenManager.Instance != null
            ? Mathf.Clamp(ModeZenManager.Instance.StageActuel, 0, 31)
            : 0;

        Debug.Log($"[ZenHaiku] StageActuel = {stageIndex}");

        bool estHaiku = stageIndex % 2 == 0;
        string langue = PlayerPrefs.GetInt("Langue", 0) == 0 ? "FR" : "EN";

        string principal;
        string secondaire;
        string type;
        Color couleur;

        if (estHaiku)
        {
            int index = (stageIndex / 2) % haikusFR.Length;
            if (langue == "FR")
            {
                principal = haikusFR[index];
                secondaire = haikuEN[index];
            }
            else
            {
                principal = haikuEN[index];
                secondaire = haikusFR[index];
            }
            type = langue == "FR" ? "— Haïku —" : "— Haiku —";
            couleur = couleurHaiku;
        }
        else
        {
            int index = ((stageIndex - 1) / 2) % citationsFR.Length;
            if (langue == "FR")
            {
                principal = citationsFR[index];
                secondaire = citationsEN[index];
            }
            else
            {
                principal = citationsEN[index];
                secondaire = citationsFR[index];
            }
            type = langue == "FR" ? "— Citation —" : "— Quote —";
            couleur = couleurCitation;
        }

        textePrincipal.text = principal;
        texteSecondaire.text = secondaire;
        texteType.text = type;

        textePrincipal.color = new Color(couleur.r, couleur.g, couleur.b, 0f);
        texteSecondaire.color = new Color(couleur.r * 0.7f, couleur.g * 0.7f, couleur.b * 0.7f, 0f);
        texteType.color = new Color(couleur.r * 0.5f, couleur.g * 0.5f, couleur.b * 0.5f, 0f);

        yield return StartCoroutine(Fondu(1f, dureeFonduIn, couleur));
        yield return new WaitForSeconds(dureeVisible);
        yield return StartCoroutine(Fondu(0f, dureeFonduOut, couleur));
    }

    IEnumerator Fondu(float alphaTarget, float duree, Color couleur)
    {
        float alphaDepart = alphaTarget == 1f ? 0f : 1f;
        float timer = 0f;

        while (timer < duree)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(alphaDepart, alphaTarget, timer / duree);

            textePrincipal.color = new Color(couleur.r, couleur.g, couleur.b, alpha);
            texteSecondaire.color = new Color(couleur.r * 0.7f, couleur.g * 0.7f, couleur.b * 0.7f, alpha * 0.6f);
            texteType.color = new Color(couleur.r * 0.5f, couleur.g * 0.5f, couleur.b * 0.5f, alpha * 0.4f);

            yield return null;
        }
    }

    void SetAlpha(TextMeshProUGUI tmp, float alpha)
    {
        if (tmp == null) return;
        Color c = tmp.color;
        c.a = alpha;
        tmp.color = c;
    }
}