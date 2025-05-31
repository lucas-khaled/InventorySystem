using UnityEngine;

public class MaterialToggler : MonoBehaviour
{
    [SerializeField] private Renderer meshRenderer;
    [SerializeField] private Material material;

    private Material initialMaterial;

    private void Awake()
    {
        initialMaterial = meshRenderer.sharedMaterial;
    }

    public void ToggleMaterial(bool toggle) 
    {
        if(toggle)
            meshRenderer.material = material;
        else
            meshRenderer.sharedMaterial = initialMaterial;
    }
}
