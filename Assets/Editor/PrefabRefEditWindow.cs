using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class PrefabRefEditWindow : EditorWindow {
	static PrefabRefEditWindow m_currentWin;
	static GameObject m_selectGB;
	private string resPath;

	[MenuItem("EZFun/UI/PrefabRef Edit Window")]
	static void ShowWindow()
	{
		if (Selection.activeGameObject != null)
		{
			m_selectGB = Selection.activeGameObject;
			m_currentWin = (PrefabRefEditWindow)EditorWindow.GetWindow(typeof(PrefabRefEditWindow));
			m_currentWin.titleContent = new GUIContent("Add FX Node/Window Prefab Ref");
		}
		else
		{
			m_selectGB = null;
		}
	}

	void OnGUI () 
	{
		if (Selection.activeGameObject != m_selectGB && Selection.activeGameObject != null)
		{
			m_selectGB = Selection.activeGameObject;
		}

		if (m_selectGB != null) {
			resPath = EditorGUILayout.TextField ("resPath:", resPath);

			if (GUILayout.Button ("Create UI Prefab Ref")) {
				if (!string.IsNullOrEmpty (resPath)) {
					var prefabRef = m_selectGB.GetComponent<PrefabRef> ();
					if (prefabRef != null) {
						Debug.LogError ("prefabRef already exists");
						return;
					}

					if (File.Exists (Application.dataPath + "/XGame/Resources/EZFunUI/Item/PrefabRef/" + resPath + ".prefab")) {
						Debug.LogWarning ("prefab exist, will be override:" + resPath);
					}

					//create prefabRef obj
					GameObject gb = new GameObject ();
					prefabRef = gb.AddComponent<PrefabRef> ();

					//set properties for prefabRef
					prefabRef.resourcePath = resPath;
					prefabRef.isPrefabDisabled = !m_selectGB.activeSelf;

					gb.transform.parent = m_selectGB.transform.parent;
					gb.transform.name = m_selectGB.name;
					gb.transform.localPosition = m_selectGB.transform.localPosition;
					gb.transform.localScale = m_selectGB.transform.localScale;
					gb.transform.localRotation = m_selectGB.transform.localRotation;
					gb.layer = m_selectGB.layer;

					gb.transform.SetSiblingIndex (m_selectGB.transform.GetSiblingIndex ());

					//create prefab for selected obj
					PrefabUtility.CreatePrefab ("Assets/XGame/Resources/EZFunUI/Item/PrefabRef/" + resPath + ".prefab", m_selectGB);

					MonoBehaviour.DestroyImmediate (m_selectGB);
				}
			}
		} else {
			GUILayout.Label ("please select a node");
		}
	}
}
