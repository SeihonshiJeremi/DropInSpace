using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class ZenVictoireRoman : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI texteRoman;
    public RectTransform panelTransform;

    [Header("Timing")]
    public float vitesseDefilement = 30f;
    public float delaiDepart = 2f;

    private static readonly string[] roman = new string[]
    {
        "— Haïku —\n\nUne goutte tombe\ndans l'infini du cosmos\net tout recommence\n\n" +
        "A drop falls softly\ninto the endless cosmos\nall begins again",

        "— Citation —\n\n« L'espace n'est pas vide.\nIl est plein de toi. »\n\n" +
        "« Space is not empty.\nIt is full of you. »",

        "— Haïku —\n\nChaque cube coloré\nest une étoile allumée\ndans ton propre ciel\n\n" +
        "Each colored cube lit\nis a star you've just ignited\nin your own night sky",

        "— Citation —\n\n« Chaque voyage commence\npar une chute volontaire. »\n\n" +
        "« Every journey begins\nwith a willful fall. »",

        "— Haïku —\n\nLe vide entre deux astres\nn'est pas le néant\nc'est le chemin\n\n" +
        "The void between stars\nis not emptiness at all\nit is the pathway",

        "— Citation —\n\n« La lumière ne se presse pas —\net pourtant, elle arrive toujours. »\n\n" +
        "« Light does not hurry —\nand yet, it always arrives. »",

        "— Haïku —\n\nTu tombes, tu roules\nla planète tourne sous toi\nl'univers sourit\n\n" +
        "You fall, you roll on\nthe planet turns beneath you\nthe universe smiles",

        "— Citation —\n\n« Tu n'explores pas l'espace.\nTu t'explores toi-même à travers lui. »\n\n" +
        "« You don't explore space.\nYou explore yourself through it. »",

        "— Haïku —\n\nUne goutte d'eau\nreflète toutes les étoiles\ntu es l'une d'elles\n\n" +
        "A single water drop\nreflects every star above\nyou are one of them",

        "— Citation —\n\n« Une planète sans visiteur\nest un poème sans lecteur. »\n\n" +
        "« A planet without a visitor\nis a poem without a reader. »",

        "— Haïku —\n\nLe silence spatial\nn'est pas absence de son\nmais plénitude\n\n" +
        "The silence of space\nis not the absence of sound\nbut pure wholeness",

        "— Citation —\n\n« Le cube rouge n'est pas un obstacle.\nC'est une invitation à choisir autrement. »\n\n" +
        "« The red cube is not an obstacle.\nIt is an invitation to choose differently. »",

        "— Haïku —\n\nAvancer sans peur\nmême quand le cube est rouge\nc'est ça, voyager\n\n" +
        "Move without fear now\neven when the cube is red\nthat is journeying",

        "— Citation —\n\n« Voyager, c'est accepter\nde ne pas savoir où l'on atterrit. »\n\n" +
        "« To travel is to accept\nnot knowing where you land. »",

        "— Haïku —\n\nLa station flotte\nentre deux bras de la galaxie\ncomme un rêve tenu\n\n" +
        "The station floats still\nbetween two arms of the stars\nlike a held dream",

        "— Citation —\n\n« La station tourne. Les étoiles restent.\nToi, tu choisis. »\n\n" +
        "« The station turns. The stars remain.\nYou choose. »",

        "— Haïku —\n\nChaque planète\ngarde un secret dans son cœur\nattends qu'elle parle\n\n" +
        "Every planet holds\na secret deep in its core\nwait, it will speak soon",

        "— Citation —\n\n« Il faut beaucoup de silence\npour entendre l'univers. »\n\n" +
        "« It takes a great deal of silence\nto hear the universe. »",

        "— Haïku —\n\nLa couleur monte\ndu cube vers l'atmosphère\nle voyage commence\n\n" +
        "Color rises up\nfrom the cube to atmosphere\nthe journey begins",

        "— Citation —\n\n« Une goutte dans l'espace reste une goutte.\nEt c'est suffisant. »\n\n" +
        "« A drop in space is still a drop.\nAnd that is enough. »",

        "— Haïku —\n\nTu n'es pas perdu\ntu explores simplement\nl'espace entre toi\n\n" +
        "You are not lost here\nyou are simply exploring\nthe space within you",

        "— Citation —\n\n« La gravité est une règle.\nLe voyage est un choix. »\n\n" +
        "« Gravity is a rule.\nTravel is a choice. »",

        "— Haïku —\n\nUne lumière lointaine\na quitté son étoile morte\nelle brille pour toi\n\n" +
        "A distant light left\nits already dead star\nit shines just for you",

        "— Citation —\n\n« Chaque étoile que tu vois\nest un soleil pour quelqu'un d'autre. »\n\n" +
        "« Every star you see\nis someone else's sun. »",

        "— Haïku —\n\nLe cosmos respire\nà un rythme que tu sens\nquand tu te tais\n\n" +
        "The cosmos breathes slow\nat a rhythm you can feel\nwhen you grow quiet",

        "— Citation —\n\n« Le chemin optimal n'est pas toujours le plus court —\nc'est celui que tu choisis. »\n\n" +
        "« The optimal path is not always the shortest —\nit is the one you choose. »",

        "— Haïku —\n\nTomber dans l'espace\nn'est pas chuter — c'est voler\nsans le savoir encore\n\n" +
        "Falling through space now\nis not falling — it is flight\nyou just don't know yet",

        "— Citation —\n\n« L'infini ne fait pas peur\nà celui qui avance un cube à la fois. »\n\n" +
        "« Infinity does not frighten those\nwho move one cube at a time. »",

        "— Haïku —\n\nLa lumière voyage\nmille ans avant d'être vue\ntu l'as vue ce soir\n\n" +
        "Light travels so far\na thousand years before seen\nyou saw it tonight",

        "— Citation —\n\n« Tu arrives toujours quelque part.\nMême dans le vide. »\n\n" +
        "« You always arrive somewhere.\nEven in the void. »",

        "— Haïku —\n\nLa dernière planète\nn'est jamais la dernière\nil y a toujours plus loin\n\n" +
        "The last planet seen\nis never truly the last\nthere is always more",

        "— Citation —\n\n« La lumière de cette planète\nt'attendait depuis le début. »\n\n" +
        "« The light of this planet\nhas been waiting for you all along. »",
    };

    void Awake()
    {
        if (panelTransform != null)
            panelTransform.anchoredPosition = new Vector2(0f, -Screen.height * 2f);
    }

    void Start()
    {
        string romanComplet = string.Join("\n\n\n---\n\n\n", roman);
        texteRoman.text = romanComplet;

        StartCoroutine(DefileRoman());
        AppliquerVignette();

    }

    IEnumerator DefileRoman()
    {

        yield return new WaitForSeconds(delaiDepart);

        UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(panelTransform);
        yield return new WaitForEndOfFrame();

        float hauteurTexte = texteRoman.preferredHeight;
        float hauteurEcran = Screen.height;

        // Commence en bas de l'écran
        panelTransform.anchoredPosition = new Vector2(0f, -hauteurEcran * 0.5f);

        while (panelTransform.anchoredPosition.y < hauteurTexte)
        {
            panelTransform.anchoredPosition += new Vector2(0f, vitesseDefilement * Time.deltaTime);
            yield return null;
        }

        // Attend 2s puis retourne au menu
        yield return new WaitForSeconds(2f);
        PlayerPrefs.SetInt("ModeZenActif", 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene("StartDropInSpace");
    }

    void AppliquerVignette()
    {
        GameObject vo = new GameObject("ZenVictoireVolume");
        var volume = vo.AddComponent<UnityEngine.Rendering.Volume>();
        volume.isGlobal = true;
        volume.priority = 10;

        var profile = ScriptableObject.CreateInstance<UnityEngine.Rendering.VolumeProfile>();

        var vignette = profile.Add<UnityEngine.Rendering.Universal.Vignette>(true);
        vignette.active = true;
        vignette.intensity.value = 0.4f;
        vignette.smoothness.value = 0.6f;
        vignette.color.value = new Color(0.3f, 0.15f, 0.05f);
        vignette.rounded.value = true;
        vignette.center.value = new Vector2(0.5f, 0.5f);
        vignette.intensity.overrideState = true;
        vignette.smoothness.overrideState = true;
        vignette.color.overrideState = true;
        vignette.rounded.overrideState = true;
        vignette.center.overrideState = true;

        var ca = profile.Add<UnityEngine.Rendering.Universal.ColorAdjustments>(true);
        ca.active = true;
        ca.colorFilter.value = new Color(1f, 0.92f, 0.75f);
        ca.colorFilter.overrideState = true;

        volume.profile = profile;
    }
}