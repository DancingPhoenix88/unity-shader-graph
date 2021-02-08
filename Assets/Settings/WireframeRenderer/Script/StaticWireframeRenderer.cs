using System.Collections.Generic;
using UnityEngine;

// ReSharper disable once UnusedMember.Global
[RequireComponent( typeof( MeshFilter ) )]
public class StaticWireframeRenderer: MonoBehaviour {
    
    private const float Distance = 1.0001f;
    private static readonly Color Color = Color.black;
    private List<Vector3> _renderingQueue;

    // ReSharper disable once MemberCanBePrivate.Global
    public Material WireMaterial;
    [Range(1, 10)] public int width = 1;

    private void InitializeOnDemand() {
        if (_renderingQueue != null) {
            return;
        }
        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
        if ( meshFilter == null ) {
            Debug.LogError( "No mesh detected at" + gameObject.name, gameObject );
            return;
        }
        Mesh mesh = meshFilter.mesh;

        _renderingQueue = new List<Vector3>();
        foreach (var point in mesh.triangles) {
            _renderingQueue.Add( mesh.vertices[point] * Distance );
        }
    }

    // ReSharper disable once UnusedMember.Global
    public void OnPreRender() {
        GL.wireframe = true;
    }

    // ReSharper disable once UnusedMember.Global
    public void OnRenderObject() {
        InitializeOnDemand();

        if (WireMaterial != null) {
            WireMaterial.SetPass(0);
        } else {
            GL.Color( Color );
        }

        GL.MultMatrix( transform.localToWorldMatrix );

        if (width == 1) {
            GL.Begin( GL.LINES );
            for ( int i = 0; i < _renderingQueue.Count; i+=3 ) {
                Vector3 vertex1 = _renderingQueue[i];
                Vector3 vertex2 = _renderingQueue[i+1];
                Vector3 vertex3 = _renderingQueue[i+2];
                GL.Vertex( vertex1 );
                GL.Vertex( vertex2 );
                GL.Vertex( vertex2 );
                GL.Vertex( vertex3 );
                GL.Vertex( vertex3 );
                GL.Vertex( vertex1 );
            }
            GL.End();
        } else {
            Vector3 camDir = transform.worldToLocalMatrix.MultiplyVector( Camera.main.transform.forward );
            GL.Begin( GL.QUADS );
            for ( int i = 0; i < _renderingQueue.Count; i+=3 ) {
                Vector3 vertex1 = _renderingQueue[i];
                Vector3 vertex2 = _renderingQueue[i+1];
                Vector3 vertex3 = _renderingQueue[i+2];
                
                Vector3 offset = Vector3.Cross(vertex2 - vertex1, camDir).normalized * width * 0.001f;
                GL.Vertex( vertex1 - offset );
                GL.Vertex( vertex2 - offset );
                GL.Vertex( vertex2 + offset );
                GL.Vertex( vertex1 + offset );

                offset = Vector3.Cross(vertex3 - vertex2, camDir).normalized * width * 0.001f;
                GL.Vertex( vertex2 - offset );
                GL.Vertex( vertex3 - offset );
                GL.Vertex( vertex3 + offset );
                GL.Vertex( vertex2 + offset );

                offset = Vector3.Cross(vertex1 - vertex3, camDir).normalized * width * 0.001f;
                GL.Vertex( vertex3 - offset );
                GL.Vertex( vertex1 - offset );
                GL.Vertex( vertex1 + offset );
                GL.Vertex( vertex3 + offset );
            }
            GL.End();
        }
        
    }

    // ReSharper disable once UnusedMember.Global
    public void OnPostRender() {
        GL.wireframe = false;
    }

}
