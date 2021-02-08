using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class MipmapVisualization : MonoBehaviour
{
    public Color m_ColMipmap4096 = Color.magenta;
    public Color m_ColMipmap2048 = Color.yellow;
    public Color m_ColMipmap1024 = Color.blue;
    public Color m_ColMipmap512 = Color.green;
    public Color m_ColMipmap256 = Color.red;
    public Color m_ColMipmap128 = Color.black;
    public Color m_ColMipmap64 = Color.black;
    public Color m_ColMipmap32 = Color.black;
    public Color m_ColMipmap16 = Color.black;
    public Color m_ColMipmap8 = Color.black;
    public Color m_ColMipmap4 = Color.black;
    public Color m_ColMipmap2 = Color.black;
    public Color m_ColMipmap1 = Color.black;
	public List<GameObject> m_AllGameObjects = new List<GameObject>();
	public List<Material> m_BackupMaterials = new List<Material>();
	public Material m_BackupMaterial = null;
    public Material m_ExampleMaterial = null;
	
	private bool m_FullReplace = false;
    private Texture2D m_MipmapVisTex;
	private Renderer m_SelectedRenderer;

	void Start ()
    {
        m_MipmapVisTex = new Texture2D(4096, 4096, TextureFormat.RGB24, true);
        FillTextureColor (0, m_ColMipmap4096);
        FillTextureColor (1, m_ColMipmap2048);
        FillTextureColor (2, m_ColMipmap1024);
        FillTextureColor (3, m_ColMipmap512);
        FillTextureColor (4, m_ColMipmap256);
        FillTextureColor (5, m_ColMipmap128);
        FillTextureColor (6, m_ColMipmap64);
        FillTextureColor (7, m_ColMipmap32);
        FillTextureColor (8, m_ColMipmap16);
        FillTextureColor (9, m_ColMipmap8);
        FillTextureColor (10, m_ColMipmap4);
        FillTextureColor (11, m_ColMipmap2);
        FillTextureColor (12, m_ColMipmap1);
        m_MipmapVisTex.Apply (false);
		
		CollectAllGameObjectsMaterial ();
	}
    void Update()
    {
		if (m_FullReplace)
		{
			// nothing need to do ...
		}
		else
		{
			bool hasRenderer = false;
			Renderer rd = null;
			for (int i = 0; i < Selection.transforms.Length; i++)
			{
				Transform t = Selection.transforms[i];
				rd = t.gameObject.GetComponent<Renderer>();
				if (rd != null)
					hasRenderer = true;
				break;
			}
			if (rd != m_SelectedRenderer && hasRenderer)
			{
				if (m_BackupMaterial != null)
					m_SelectedRenderer.material = m_BackupMaterial;
				
				m_BackupMaterial = rd.material;
				
				rd.material = m_ExampleMaterial;
				rd.material.SetTexture ("_MainTex", m_MipmapVisTex);
				m_SelectedRenderer = rd;
			}
		}
	}
	void FillTextureColor (int mipmap, Color c)
	{
		Color[] pixels = m_MipmapVisTex.GetPixels (mipmap);
		for (int i = 0; i < pixels.Length; i++)
			pixels[i] = c;
		m_MipmapVisTex.SetPixels (pixels, mipmap);
	}
	void OnGUI ()
	{
		GUI.Label (new Rect (10, 10, 250, 30), "Mipmap Visualization Demo");
		string s = m_FullReplace ? "Single" : "All";
		if (GUI.Button (new Rect (10, 45, 180, 30), "Replace " + s))
		{
			if (m_FullReplace)
			{
				RevertAllGameObjectsMaterial ();
			}
			else
			{
				CollectAllGameObjectsMaterial ();
				ReplaceAllGameObjectsMaterial ();
			}
			m_FullReplace = !m_FullReplace;
		}
	}
	void CollectAllGameObjectsMaterial ()
	{
		GameObject[] arrGo = Object.FindObjectsOfType (typeof(GameObject)) as GameObject[];
		for (int i = 0; i < arrGo.Length; i++)
		{
			Renderer rd = arrGo[i].GetComponent<Renderer>();
			if (rd != null)
			{
				m_AllGameObjects.Add (arrGo[i]);
				m_BackupMaterials.Add (rd.material);
			}
		}
	}
	void ReplaceAllGameObjectsMaterial ()
	{
		for (int i = 0, cnt = m_AllGameObjects.Count; i < cnt; i++)
		{
			Renderer rd = m_AllGameObjects[i].GetComponent<Renderer>();
			rd.material = m_ExampleMaterial;
			rd.material.SetTexture ("_MainTex", m_MipmapVisTex);
		}
	}
	void RevertAllGameObjectsMaterial ()
	{
		for (int i = 0, cnt = m_AllGameObjects.Count; i < cnt; i++)
		{
			Renderer rd = m_AllGameObjects[i].GetComponent<Renderer>();
			rd.material = m_BackupMaterials[i];
		}
	}
}
