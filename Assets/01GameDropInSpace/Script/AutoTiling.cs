using UnityEngine;

// [ExecuteAlways] => le script tourne aussi dans l'éditeur,
// on voit le résultat en temps réel quand on déplace/scalle un bloc
[ExecuteAlways]
[RequireComponent(typeof(Renderer))]
public class AutoTiling : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Taille en mètres d'une répétition de la texture")]
    private float textureWorldSize = 1.0f;

    private Renderer _renderer;
    private MaterialPropertyBlock _propBlock;

    // Id du scale, mis en cache pour ne pas le recalculer à chaque frame
    private static readonly int MainTexScaleId = Shader.PropertyToID("_BaseMap_ST");

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _propBlock = new MaterialPropertyBlock();
    }

    private void Update()
    {
        ApplyTiling();
    }

    private void OnValidate()
    {
        // Appelé quand on modifie l'inspecteur, pratique pour ajuster
        // textureWorldSize en live
        if (_renderer == null)
            _renderer = GetComponent<Renderer>();

        ApplyTiling();
    }

private void ApplyTiling()
{
    if (_renderer == null || _propBlock == null)
        return;

    Vector3 scale = transform.lossyScale;

    // On récupère l'état actuel du block (sinon on écrase les autres props)
    _renderer.GetPropertyBlock(_propBlock);

    // Version correcte : GetVector retourne la valeur directement
    Vector4 currentST = _propBlock.GetVector(MainTexScaleId);

    Vector4 newST = new Vector4(
        scale.x / textureWorldSize,
        scale.z / textureWorldSize,
        currentST.z, // offset X inchangé
        currentST.w  // offset Y inchangé
    );

    _propBlock.SetVector(MainTexScaleId, newST);
    _renderer.SetPropertyBlock(_propBlock);
}
}