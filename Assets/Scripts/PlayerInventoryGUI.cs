using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInventoryGUI : MonoBehaviour {
	private bool inventoryWindowToggle;
	private Rect inventoryWindowRect;

	// Item Dictionary. 
	// The player inventory starts empty and can hold up to three items. 
	// Still need to implement the possiblity of stacking items of the type
	static public Dictionary<int, string> inventoryNameDictionary = new Dictionary<int, string> ()
	{
		{0, string.Empty}, // Every collectable item will have an index number for easy id in code and a "name" displayed on screen.
		{1, string.Empty},
		{2, string.Empty}
	};

	ItemClass itemObject = new ItemClass();
	
	// Use this for initialization
	void Awake () {
		inventoryWindowToggle = false; // Start with inventory closed.
		inventoryWindowRect = new Rect(800, 200, 110, 200); // Defines space on GUI where the player inventory displays.
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		// Toggle inventory button placement
		inventoryWindowToggle = GUI.Toggle (new Rect (800, 400, 100, 50), inventoryWindowToggle, "Inventory");
		// When toggle button is pressed call showInvSlots()
		if (inventoryWindowToggle) {
			inventoryWindowRect = GUI.Window (0, inventoryWindowRect, showInvSlots, "Inventory");		
		}
	}

	//-----------------------------------------------------
	// showInvSlots
	// Displays the players inventory on to screen.
	//-----------------------------------------------------
	void showInvSlots (int windowId){

		// Inventory button layout
		GUILayout.BeginArea (new Rect(5, 25, 95, 160));

		GUILayout.BeginHorizontal ();
		GUILayout.Button (inventoryNameDictionary[0], GUILayout.Height (50));
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		GUILayout.Button (inventoryNameDictionary[1], GUILayout.Height (50));
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		GUILayout.Button (inventoryNameDictionary[2], GUILayout.Height (50));
		GUILayout.EndHorizontal ();

		GUILayout.EndArea ();

	}	
}
