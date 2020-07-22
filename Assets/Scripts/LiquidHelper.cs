using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [ExecuteInEditMode] // not working in EDIT mode since Update function is not called
public class LiquidHelper : MonoBehaviour {
    public MeshRenderer liquidOutterRenderer = null;
    public MeshRenderer liquidInnerRenderer = null;
    protected Material liquidOutterMaterial = null;
    protected Material liquidInnerMaterial = null;
    protected Transform t = null;
    protected Vector3 p = Vector3.zero;
    protected Quaternion q = Quaternion.identity;
    [SerializeField] protected float wobbleAmount = 0f;
    public float dampen = 0.1f;
    [SerializeField] protected float lastWobbleAmount = 0f;
    protected const float MIN_WOBBLE_AMOUNT = 0.001f;
    public float MAX_WOBBLE_AMOUNT = 0.26f;
    protected const float STAY_THRESHOLD = 0.0001f;
    //----------------------------------------------------------------------------------------------
    protected void Awake () {
        t = transform;
        p = t.position;
    }
    //----------------------------------------------------------------------------------------------
    protected void Update() {
        if (!CacheRefs()) return;

        // Reduce wobble amount if the potion stays
        bool isStaying = ((p - t.position).sqrMagnitude < STAY_THRESHOLD) && (q == t.rotation);
        if (isStaying) {
            wobbleAmount -= (dampen * Time.deltaTime);
            if (wobbleAmount < MIN_WOBBLE_AMOUNT) wobbleAmount = 0f;
        } else {
            wobbleAmount = Mathf.Min( wobbleAmount + dampen * Time.deltaTime, MAX_WOBBLE_AMOUNT );
        }
        p = t.position;
        q = t.rotation;

        SetWobbleAmount();
    }
    //----------------------------------------------------------------------------------------------
    protected void SetWobbleAmount () {
        if (Mathf.Approximately( wobbleAmount, lastWobbleAmount )) return;
        liquidOutterMaterial.SetFloat( "WobbleAmount", wobbleAmount );
        liquidInnerMaterial.SetFloat( "WobbleAmount", wobbleAmount );
        lastWobbleAmount = wobbleAmount;

        // Debug.LogFormat( "[LiquidHelper] Wobble amount = {0}", wobbleAmount );
    }
    //----------------------------------------------------------------------------------------------
    protected bool CacheRefs () {
        if (t == null) {
            Debug.LogError( "[LiquidHelper] Transform not found" );
            return false;
        }

        // Renderers
        if (liquidOutterRenderer == null) {
            Debug.LogError( "[LiquidHelper] MeshRenderer for Outter not found" );
            return false;
        }
        if (liquidInnerRenderer == null) {
            Debug.LogError( "[LiquidHelper] MeshRenderer for Inner not found" );
            return false;
        }

        // Materials
        if (liquidOutterMaterial == null) {
            if (Application.isPlaying) liquidOutterMaterial = liquidOutterRenderer.material;
            else liquidOutterMaterial = liquidOutterRenderer.sharedMaterial;
        }
        if (liquidOutterMaterial == null) {
            Debug.LogError( "[LiquidHelper] Liquid Outter material not found" );
            return false;
        }
        if (liquidInnerMaterial == null) {
            if (Application.isPlaying) liquidInnerMaterial = liquidInnerRenderer.material;
            else liquidInnerMaterial = liquidInnerRenderer.sharedMaterial;
        }
        if (liquidInnerMaterial == null) {
            Debug.LogError( "[LiquidHelper] Liquid Inner material not found" );
            return false;
        }

        return true;
    }

}
