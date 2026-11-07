# OpenTrophy pour Unity

Des trophées à la PlayStation dans ton jeu, avec le service OpenTrophy : bronze, argent, or, platine automatique,
trophées cachés, compteurs, extensions.

Le principe : **le jeu n'attend jamais le réseau**. Un trophée est gagné et enregistré sur l'appareil à l'instant,
avec ou sans connexion, avec ou sans compte. Dès que le joueur a lié son compte et que le serveur répond, tout ce
qui a été gagné part avec sa vraie date.

Unity 2021.3 ou plus récent. Aucune dépendance (ni Newtonsoft, ni TextMeshPro, ni canvas).

## Installation

1. Copie le dossier `com.sekaix.opentrophy` dans le dossier `Packages/` de ton projet
   (ou Package Manager › **+** › *Add package from disk…* › `package.json`).
2. Ouvre **Tools › OpenTrophy**, clique **Créer les réglages**.
3. Sur le site OpenTrophy, espace développeur › ton jeu › **Clés et intégration** : crée une clé, colle-la dans
   **Clé du jeu**.
4. Clique **Vérifier et récupérer les trophées** : la liste est embarquée dans le jeu.
5. Clique **Générer TrophyKeys.cs** pour avoir une constante par trophée.

## Dans le code

```csharp
using OpenTrophy;

Trophy.Unlock(TrophyKeys.FirstBoss);     // un trophée
Trophy.AddProgress(TrophyKeys.Hunter);   // un compteur : +1 (débloque en atteignant la cible)
Trophy.SetProgress(TrophyKeys.Hunter, 37);
Trophy.ShowLinkPanel();                  // le joueur lie (ou délie) son compte
```

C'est tout ce qu'il faut. Le platine ne se débloque jamais à la main : il tombe quand tous les trophées de la liste
principale sont gagnés (pas pendant une bêta).

Pour un écran de trophées maison :

```csharp
foreach (TrophyStatus t in Trophy.GetAll())
{
    string nom = t.Secret ? "Trophée caché" : t.Def.Name;   // un trophée caché ne dit rien avant d'être gagné
    bool gagne = t.Unlocked;                                // t.UnlockedAt, t.Progress, t.Def.Target, t.Def.Tier, t.Def.Group
}
Trophy.LoadIcon(def, locked: !t.Unlocked, tex => image.texture = tex);   // icône débloquée ou bloquée, mise en cache
Trophy.Changed += Redessiner;            // listes, compteurs ou liaison ont changé
Trophy.Unlocked += def => { ... };       // un trophée vient de tomber : pour ta propre notification
```

La notification intégrée (coin de l'écran, durée, son, textes) se règle dans la fenêtre ; décoche-la pour faire la tienne.

## Langues

Les traductions saisies sur OpenTrophy (dans « Modifier » de chaque trophée, ou par le fichier .json) arrivent toutes avec la liste : le jeu change de langue sans rien redemander au serveur, même hors ligne.

- Par défaut, les trophées sont montrés dans la langue du système du joueur. Pour en imposer une : champ **Langue** des réglages (`en`, `pt-BR`…).
- Quand le joueur change la langue du jeu : `Trophy.Language = "en";` — `Name` et `Description` changent tout de suite et `Trophy.Changed` est levé.
- `Trophy.Languages` donne les langues disponibles, celle du jeu en premier.
- Un trophée sans traduction reste dans la langue d'écriture du jeu (`BaseName`, `BaseDescription`).

Après avoir ajouté des traductions sur le site, rouvre **Tools › OpenTrophy** et recharge la liste pour qu'elles soient embarquées dans le jeu.

## Lier le compte du joueur

`Trophy.ShowLinkPanel()` affiche un panneau prêt à l'emploi : il montre un code (`ABCD-EFGH`), le joueur ouvre la
page et l'accepte sur le site, le jeu le reconnaît en quelques secondes. Pour ta propre interface :
`Trophy.BeginLink()`, puis les événements `LinkCodeReady(code, url)`, `Linked(nom)`, `LinkFailed(raison)`,
et `Trophy.OpenLinkPage()`, `Trophy.CancelLink()`, `Trophy.Unlink()`.

Tant qu'aucun compte n'est lié, les trophées restent sur l'appareil. À la liaison, **tout** ce qui a été gagné
est envoyé. Si le joueur retire le lien depuis le site, le jeu le remarque et repasse en local sans rien perdre.

## Le fichier local

`<persistentDataPath>/opentrophy/trophies.dat`, chiffré (AES-256) et signé (HMAC-SHA256). Sa clé vient du
**secret local** des réglages (généré pour ton jeu), de la clé du jeu et de l'identifiant de l'appareil :

- modifié à la main, il est rejeté (mis de côté en `.rejected`) ;
- copié depuis une autre machine, il ne s'ouvre pas ;
- une écriture coupée (coupure de courant) retombe sur la version précédente.

Ce que ça vaut : ça arrête la triche facile. Le secret est forcément dans le jeu, donc quelqu'un qui le démonte peut
le retrouver — c'est vrai de tous les systèmes de succès côté client.
Ne change plus le secret local une fois le jeu sorti : les joueurs sans compte lié perdraient leurs trophées locaux.
Si un joueur change de matériel sans avoir lié de compte, ses trophées locaux ne suivent pas ; avec un compte, ils
reviennent du serveur.

## Tester dans l'éditeur

En mode Play, la fenêtre **Tools › OpenTrophy** liste les trophées avec un bouton **Débloquer** (ou **+N** pour un
compteur), l'état de la liaison, le nombre d'envois en attente, et **Effacer le local** pour recommencer.
Pour un jeu encore en brouillon sur le site, seul ton compte développeur peut se lier.
L'exemple *Démo* (Package Manager › OpenTrophy › Samples) fait la même chose dans le jeu.

## À savoir

- Les icônes sont demandées au serveur en PNG (Unity ne lit pas le WebP) et gardées sur le disque.
- Le paquet utilise les modules Unity intégrés IMGUI, UnityWebRequest, UnityWebRequestTexture, ImageConversion et
  Audio (activés par défaut).
- WebGL : le fichier local vit dans le stockage du navigateur ; non essayé.
- Le cœur (`Runtime/Core`) n'utilise pas Unity : il est testé hors Unity contre un vrai serveur (`Tests~/run.mjs`).
