using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

[VFXBinder("Material/Property (Texture)")]
public class MaterialPropertyTextureBinder : VFXBinderBase {
    [VFXPropertyBinding("UnityEngine.Texture2D")]
    public ExposedProperty vfxProperty;

    [SerializeField] protected MeshRenderer meshRenderer;
    [SerializeField] protected string propertyName;

    [SerializeField] protected bool useSharedMaterial = false;
    [SerializeField] protected bool forceReset = false;
    protected Material material;
    protected int propertyId = -1;

    public override bool IsValid (VisualEffect component) {
        if (meshRenderer == null) return false;
        if (component.HasTexture( vfxProperty ) == false) return false;

        Init();
        return material != null;
    }

    public override void UpdateBinding (VisualEffect component) {
        Init();
        component.SetTexture( vfxProperty, material.GetTexture( propertyId ) );
    }

    protected void Init () {
        if (material == null || forceReset) {
            material = useSharedMaterial ? meshRenderer.sharedMaterial : meshRenderer.material;
            propertyId = Shader.PropertyToID( propertyName );
        }
    }
}