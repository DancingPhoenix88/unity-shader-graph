using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LiquidRotationSupport : MonoBehaviour {
    protected MeshRenderer liquidRenderer = null;
    protected Material liquidMaterial = null;
    protected Transform t = null;
    protected Quaternion q = Quaternion.identity;
    //----------------------------------------------------------------------------------------------
    protected void Awake () {
        t = transform;
    }
    //----------------------------------------------------------------------------------------------
    protected void Update() {
        if (!CacheRefs()) return;

        // Skip if rotation does not change
        if (q == t.rotation) {
            // Debug.Log( "[LiquidRotationSupport] Unchanged -> skip" );
            return;
        }
        q = t.rotation;

        // Update Shader parameters
        Bounds bounds = liquidRenderer.bounds;
        float y0 = t.position.y;
        Vector2 yRange = new Vector2( bounds.min.y - y0, bounds.max.y - y0 );
        liquidMaterial.SetVector( "YRange", yRange );
        // Debug.LogFormat( "[LiquidRotationSupport]: {0}", yRange );
    }
    //----------------------------------------------------------------------------------------------
    protected bool CacheRefs () {
        if (t == null) return false;

        // Renderer
        if (liquidRenderer == null) {
            liquidRenderer = t.GetComponent<MeshRenderer>();
        }
        if (liquidRenderer == null) {
            Debug.LogError( "[LiquidRotationSupport] Transform or MeshRenderer not found" );
            return false;
        }

        // Material
        if (liquidMaterial == null) {
            if (Application.isPlaying) liquidMaterial = liquidRenderer.material;
            else liquidMaterial = liquidRenderer.sharedMaterial;
        }
        if (liquidMaterial == null) {
            Debug.LogError( "[LiquidRotationSupport] Liquid material not found" );
            return false;
        }

        return true;
    }
    //----------------------------------------------------------------------------------------------
    protected void OnDrawGizmosSelected () {
        if (t == null || liquidRenderer == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube( t.position, liquidRenderer.bounds.size );
    }
}
