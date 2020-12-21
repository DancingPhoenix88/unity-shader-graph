using UnityEngine;

[ExecuteInEditMode]
public class CustomBillboard : MonoBehaviour {
    protected Transform t;
    protected Transform tCamera;
    protected void Awake () {
        t = transform;
        tCamera = Camera.main.transform;
    }

    protected void Update() {
        if (t != null && tCamera != null) {
            t.LookAt( tCamera, Vector3.up );
        }
    }
}
