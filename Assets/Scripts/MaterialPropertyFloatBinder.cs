using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

[VFXBinder("Material/Property (Float)")]
public class MaterialPropertyFloatBinder : VFXBinderBase {
    [VFXPropertyBinding("System.Single")]
    public ExposedProperty vfxProperty;

    [SerializeField] protected MeshRenderer meshRenderer;
    [SerializeField] protected string propertyName;

    [SerializeField] protected bool useSharedMaterial = false;
    [SerializeField] protected bool forceReset = false;
    protected Material material;
    protected int propertyId = -1;

    public override bool IsValid (VisualEffect component) {
        if (meshRenderer == null) return false;
        if (component.HasFloat( vfxProperty ) == false) return false;

        Init();
        return material != null;
    }

    public override void UpdateBinding (VisualEffect component) {
        Init();
        component.SetFloat( vfxProperty, material.GetFloat( propertyId ) );
    }

    protected void Init () {
        if (material == null || forceReset) {
            material = useSharedMaterial ? meshRenderer.sharedMaterial : meshRenderer.material;
            propertyId = Shader.PropertyToID( propertyName );
        }
    }
}