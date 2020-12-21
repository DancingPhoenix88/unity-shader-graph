using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CurvedLineRenderer : MonoBehaviour {
    public const int MAX_POINTS = 512;
    private LineRenderer lineRenderer;
    protected Transform t;
    public Transform target;
    public Transform bend1;
    public Transform bend2;
    [Range(20, MAX_POINTS)] public int pointsCount;
    private Vector3[] points;
    private Vector3[] anchors;
    private int oldPointsCount;
    protected bool init = false;
    //------------------------------------------------------------------------------------------------------------------
    protected void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        t = transform;
        anchors = new Vector3[4];
        points = new Vector3[MAX_POINTS];
        init = true;
    }
    //------------------------------------------------------------------------------------------------------------------
    protected void Update() {
        if (init == false) return;
        DrawQuadraticBezierCurve( t.position, bend1.position, bend2.position, target.position );
    }
    //------------------------------------------------------------------------------------------------------------------
    protected void DrawQuadraticBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) {
        if (IsDirty( p0, p1, p2, p3 ) == false) return;

        float t = 0f;
        float step = 1f / pointsCount;
        float omT = 1f; // one minus T
        for (int i = 0; i < pointsCount; i++) {
            points[i] = omT * omT * omT * p0
                        + 3 * omT * omT * t * p1
                        + 3 * omT * t * t * p2
                        + t * t * t * p3;
            t += step;
            omT = 1f - t;
        }

        lineRenderer.SetPositions( points );
        lineRenderer.positionCount = oldPointsCount = pointsCount;
    }
    //------------------------------------------------------------------------------------------------------------------
    private bool IsDirty (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) {
        return (pointsCount != oldPointsCount
         || Mathf.Approximately( (anchors[0] - p0).sqrMagnitude, 0f ) == false
         || Mathf.Approximately( (anchors[1] - p1).sqrMagnitude, 0f ) == false
         || Mathf.Approximately( (anchors[2] - p2).sqrMagnitude, 0f ) == false
         || Mathf.Approximately( (anchors[3] - p3).sqrMagnitude, 0f ) == false
        );
    }
}