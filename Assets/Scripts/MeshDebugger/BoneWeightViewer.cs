using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[ExecuteInEditMode]
public class BoneWeightViewer : MonoBehaviour {
    #if UNITY_EDITOR
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public Gradient gradient;
    //------------------------------------------------------------------------------------------------------------------
    protected Transform[] bones;
    protected Mesh mesh;
    protected List<Vector3> vertices;
    protected List<BoneWeight> weights;
    protected Transform t;
    protected Matrix4x4 localToWorld;
    protected int thisBoneIndex = -1;
    //------------------------------------------------------------------------------------------------------------------
    protected bool Init () {
        if (weights != null) return true;

        if (skinnedMeshRenderer == null) {
            Debug.LogWarning( "skinnedMeshRenderer is null -> exit" );
            return false;
        }
        
        t               = transform;
        bones           = skinnedMeshRenderer.bones;
        thisBoneIndex   = System.Array.IndexOf( bones, t );
        
        mesh        = new Mesh();
        vertices    = new List<Vector3>();
        weights     = new List<BoneWeight>();
        skinnedMeshRenderer.sharedMesh.GetBoneWeights( weights );

        return true;
    }
    //------------------------------------------------------------------------------------------------------------------
    protected void UpdateMeshPos () {
        skinnedMeshRenderer.BakeMesh( mesh );
        localToWorld = skinnedMeshRenderer.localToWorldMatrix;
        localToWorld.SetTRS( localToWorld.GetColumn(3), localToWorld.rotation, Vector3.one );
        mesh.GetVertices( vertices );
    }
    //------------------------------------------------------------------------------------------------------------------
    protected void OnDrawGizmosSelected () {
        if (Init() == false) return;
        UpdateMeshPos();

        Gizmos.color = Color.green;
        for (int i = 0; i < t.childCount; ++i) {
            Gizmos.DrawLine( t.position, t.GetChild(i).position );
        }

        for (int i = 0; i < vertices.Count; ++i) {
            BoneWeight ws = new BoneWeight();
            if (i < weights.Count) ws = weights[i];
            if (ws.boneIndex0 == thisBoneIndex) DrawWeightedVertex( vertices[i], ws.weight0 );
            else if (ws.boneIndex1 == thisBoneIndex) DrawWeightedVertex( vertices[i], ws.weight1 );
            else if (ws.boneIndex2 == thisBoneIndex) DrawWeightedVertex( vertices[i], ws.weight2 );
            else if (ws.boneIndex3 == thisBoneIndex) DrawWeightedVertex( vertices[i], ws.weight3 );
            else DrawWeightedVertex( vertices[i], 0f );
        }
    }
    //------------------------------------------------------------------------------------------------------------------
    protected Vector3 GetVertexWorldPos (Vector3 v) {
        return localToWorld.MultiplyPoint3x4( v );
    }
    //------------------------------------------------------------------------------------------------------------------
    protected void DrawWeightedVertex (Vector3 v, float w) {
        Gizmos.color = gradient.Evaluate( w );
        Gizmos.DrawSphere( GetVertexWorldPos( v ), Mathf.Max(.005f * w, 0.001f) );
    }
    #endif
}
