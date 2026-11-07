using UnityEngine;

namespace OpenTrophy
{
    public enum ToastCorner { TopRight, TopLeft, BottomRight, BottomLeft, TopCenter, BottomCenter }

    /// <summary>The words shown by the built-in notification and link panel. Change them to translate the game.</summary>
    [System.Serializable]
    public sealed class OpenTrophyTexts
    {
        public string trophyUnlocked = "Trophée obtenu";
        public string platinumUnlocked = "Platine obtenu";
        public string hiddenTrophy = "Trophée caché";
        public string linkTitle = "Compte OpenTrophy";
        public string linkIntro = "Lie ton compte pour retrouver tes trophées sur le site et sur tes autres appareils. Tes trophées déjà gagnés seront envoyés.";
        public string linkButton = "Lier mon compte";
        public string linkCodeHelp = "Ouvre la page et saisis ce code, ou clique sur le bouton :";
        public string openPage = "Ouvrir la page";
        public string waiting = "En attente de ta validation sur le site…";
        public string linkedAs = "Compte lié : {0}";
        public string unlink = "Délier ce compte";
        public string cancel = "Annuler";
        public string close = "Fermer";
        public string offline = "Serveur injoignable. Tes trophées restent enregistrés sur cet appareil.";
        public string expired = "Le code a expiré. Redemande-en un.";
        public string error = "La liaison a échoué. Réessaie plus tard.";
        public string pending = "{0} en attente d'envoi";
    }

    /// <summary>
    /// The game's OpenTrophy configuration. One asset, in a Resources folder, named OpenTrophySettings.
    /// Create and fill it from the menu Tools › OpenTrophy.
    /// </summary>
    public sealed class OpenTrophySettings : ScriptableObject
    {
        public const string ResourceName = "OpenTrophySettings";

        [Tooltip("Adresse du service, sans barre finale.")]
        public string serverUrl = "https://opentrophy.sekaix.fr";
        [Tooltip("Clé du jeu (otg_…), créée dans l'espace développeur. Elle ne débloque rien à elle seule.")]
        public string gameKey = "";
        [Tooltip("Texte aléatoire propre à ce jeu : il entre dans la clé du fichier local. Ne le change plus une fois le jeu sorti.")]
        public string localSecret = "";
        [Tooltip("La liste des trophées embarquée dans le jeu (remplie par la fenêtre OpenTrophy), utilisée hors ligne au premier lancement.")]
        [TextArea(2, 6)] public string embeddedDefinitions = "";

        [Header("Langue")]
        [Tooltip("Langue des trophées : un code (en, es, pt-BR…). Vide : la langue du système du joueur. Les trophées sans traduction restent dans la langue du jeu. Se change aussi en cours de jeu avec Trophy.Language.")]
        public string language = "";

        [Header("Notification")]
        [Tooltip("Affiche la notification intégrée quand un trophée est gagné. Décoche pour faire la tienne avec Trophy.Unlocked.")]
        public bool showToasts = true;
        public ToastCorner toastCorner = ToastCorner.TopRight;
        [Range(2f, 12f)] public float toastSeconds = 5f;
        [Tooltip("Son joué avec la notification (facultatif).")]
        public AudioClip toastSound;
        [Range(0f, 1f)] public float toastVolume = 0.8f;

        [Header("Divers")]
        [Tooltip("Écrit dans la console ce que fait OpenTrophy.")]
        public bool verboseLog = false;
        public OpenTrophyTexts texts = new OpenTrophyTexts();

        public static OpenTrophySettings Load() { return Resources.Load<OpenTrophySettings>(ResourceName); }
        public bool IsConfigured { get { return !string.IsNullOrEmpty(serverUrl) && !string.IsNullOrEmpty(gameKey); } }
    }
}
