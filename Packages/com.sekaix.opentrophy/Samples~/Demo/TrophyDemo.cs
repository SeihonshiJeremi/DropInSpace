using System.Collections.Generic;
using OpenTrophy;
using OpenTrophy.Core;
using UnityEngine;

/// <summary>
/// Pose ce script sur un objet d'une scène vide, lance le jeu : une liste de tes trophées apparaît,
/// avec un bouton pour débloquer chacun. Montre tout ce dont un jeu a besoin.
/// </summary>
public class TrophyDemo : MonoBehaviour
{
    List<TrophyStatus> trophies = new List<TrophyStatus>();
    Vector2 scroll;

    void OnEnable() { Trophy.Changed += Refresh; Trophy.Unlocked += OnUnlocked; Refresh(); }
    void OnDisable() { Trophy.Changed -= Refresh; Trophy.Unlocked -= OnUnlocked; }

    void Refresh() { trophies = Trophy.GetAll(); }
    void OnUnlocked(TrophyDef def) { Debug.Log("Trophée gagné : " + def.Name + " (" + def.Tier + ")"); }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(20, 20, 460, Screen.height - 40), GUI.skin.box);
        GUILayout.Label(Trophy.IsReady ? (Trophy.Game != null ? Trophy.Game.Title : "Liste des trophées pas encore reçue") : "OpenTrophy n'est pas configuré (Tools › OpenTrophy)");
        GUILayout.Label(Trophy.IsLinked ? "Compte : " + Trophy.PlayerName : "Aucun compte lié : les trophées restent sur cet appareil");
        if (GUILayout.Button(Trophy.IsLinked ? "Compte…" : "Lier mon compte")) Trophy.ShowLinkPanel();
        scroll = GUILayout.BeginScrollView(scroll);
        foreach (TrophyStatus t in trophies)
        {
            GUILayout.BeginHorizontal();
            // Un trophée caché ne dit rien de lui tant qu'il n'est pas gagné.
            string label = t.Secret ? "Trophée caché" : t.Def.Name + (t.Def.Target > 0 ? "  " + t.Progress + "/" + t.Def.Target : "");
            GUILayout.Label((t.Unlocked ? "✔ " : "   ") + label, GUILayout.Width(300));
            if (!t.Unlocked && t.Def.Tier != TrophyTier.Platinum)
            {
                if (t.Def.Target > 0) { if (GUILayout.Button("+10")) Trophy.AddProgress(t.Def.Key, 10); }
                else if (GUILayout.Button("Débloquer")) Trophy.Unlock(t.Def.Key);
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }
}
