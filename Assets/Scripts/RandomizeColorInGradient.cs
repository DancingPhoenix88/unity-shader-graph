using UnityEngine;
using UnityEngine.UI;

public class RandomizeColorInGradient : MonoBehaviour {
    public MeshRenderer meshRenderer;
    public float duration = 1f;
    public Gradient[] gradients;
    protected MaterialPropertyBlock materialPropertyBlock;
    protected float progress = 0f;
    protected int[] shaderParams;
    //------------------------------------------------------------------------------------------------------------------
    protected void Start () {
        materialPropertyBlock = new MaterialPropertyBlock();
        shaderParams = new int[]{
            Shader.PropertyToID( "ColorPrimary" ),
            Shader.PropertyToID( "ColorSecondary" ),
            Shader.PropertyToID( "ColorHighlight" ),
            Shader.PropertyToID( "ColorAlbedo" )
        };
    }
    //------------------------------------------------------------------------------------------------------------------
    protected void Update () {
        progress = (progress + Time.deltaTime) % duration;
        for (var i = 0; i < shaderParams.Length; ++i) {
            materialPropertyBlock.SetColor( shaderParams[i], gradients[i].Evaluate( progress / duration ) );
        }
        meshRenderer.SetPropertyBlock( materialPropertyBlock );
    }
}
