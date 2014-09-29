using UnityEngine;
using System.Collections;

public class PlayerInventory : MonoBehaviour {
	public Rect invBackground;

	// Use this for initialization
	void Start () {
		invBackground = new Rect(10, 250, 105, 200);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		invBackground = GUI.Window (0, invBackground, showInvSlots, "Inventory");
	}

	void showInvSlots (int windowId){
		GUILayout.BeginArea (new Rect(5, 25, 95, 160));

		GUILayout.BeginHorizontal ();
		GUILayout.Button ("Item 1", GUILayout.Height (50));
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		GUILayout.Button ("Item 2", GUILayout.Height (50));
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		GUILayout.Button ("Item 3", GUILayout.Height (50));
		GUILayout.EndHorizontal ();

		GUILayout.EndArea ();

	}
}
