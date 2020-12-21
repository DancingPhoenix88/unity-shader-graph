using UnityEngine;

public class AutoRotate : MonoBehaviour {
    public float speed = 0.01f;

    protected Transform t;

    protected void Awake () {
        t = transform;
    }

    protected void Update () {
        t.Rotate( Vector3.up * speed * Time.deltaTime, Space.Self );
    }
}
