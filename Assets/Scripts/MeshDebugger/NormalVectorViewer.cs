using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[ExecuteInEditMode]
public class NormalVectorViewer : MonoBehaviour {
    #if UNITY_EDITOR
    protected SkinnedMeshRenderer skinnedMeshRenderer;
    protected MeshFilter meshFilter;
    protected Mesh mesh;
    protected List<Vector3> vertices;
    protected List<Vector3> normals;
    protected Transform t;
    protected Matrix4x4 localToWorld;
    //------------------------------------------------------------------------------------------------------------------
    protected bool Init () {
        if (normals != null) {
            if (normals.Count > vertices.Count) {
                return true;
            }
        }

        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();

        if (skinnedMeshRenderer == null && meshFilter == null) {
            Debug.LogWarning( "skinnedMeshRenderer & meshFilter are null -> exit" );
            return false;
        }
        
        t           = transform;
        mesh        = new Mesh();
        vertices    = new List<Vector3>();
        normals     = new List<Vector3>();

        if (meshFilter != null) {
            mesh = meshFilter.sharedMesh;
            mesh.GetVertices( vertices );
            localToWorld = t.localToWorldMatrix;
        } else {
            skinnedMeshRenderer.BakeMesh( mesh );
        }
        mesh.GetNormals( normals );

        if (normals.Count >= vertices.Count) {
            return true;
        } else {
            Debug.LogWarningFormat( "Normal:{0} vs Vertices:{1}", normals.Count, vertices.Count );
            return false;
        }
    }
    //------------------------------------------------------------------------------------------------------------------
    protected void UpdateMeshPos () {
        if (skinnedMeshRenderer != null) {
            skinnedMeshRenderer.BakeMesh( mesh );
            mesh.GetVertices( vertices );
            localToWorld = skinnedMeshRenderer.localToWorldMatrix;
            localToWorld.SetTRS( localToWorld.GetColumn(3), localToWorld.rotation, Vector3.one );
        }
    }
    //------------------------------------------------------------------------------------------------------------------
    protected void OnDrawGizmosSelected () {
        if (Init() == false) return;
        UpdateMeshPos();

        for (int i = 0; i < vertices.Count; ++i) {
            Vector3 v = vertices[i];
            v = GetVertexWorldPos( v );
            Gizmos.color = Color.red;
            Gizmos.DrawSphere( v, 0.01f );
            Gizmos.color = Color.green;
            Gizmos.DrawLine( v, v + normals[i] * .1f );
        }
    }
    //------------------------------------------------------------------------------------------------------------------
    protected Vector3 GetVertexWorldPos (Vector3 v) {
        return localToWorld.MultiplyPoint3x4( v );
    }
    #endif
}
