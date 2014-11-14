using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Attach to Player in Unity
public class PlayerInventoryGUI : MonoBehaviour {
	static public bool isOpen;
	private Rect inventoryWindowRect;

	public GameObject Beehive;
	GameObject clone;
	private Transform playerTransform; // Player position
	
	// The player inventory starts empty and can hold up to three items. 
	static public Dictionary<int, string> inventoryNameDictionary = new Dictionary<int, string> ()
	{	
		{0, string.Empty},
		{1, string.Empty},
		{2, string.Empty}
	};

	// Use this for initialization
	void Awake () {
		isOpen = false;
		clone = null;
		inventoryWindowRect = new Rect(800, 200, 110, 200);
		playerTransform = GetComponent<Transform> ();
	}
	
	// Update is called once per frame
	void Update () {

		InvToggle();
		Equip();
	}

	void OnGUI(){

		// When inventory isOpen call showInvSlots()
		if (isOpen) {
			inventoryWindowRect = GUI.Window (0, inventoryWindowRect, showInvSlots, "Inventory");		
		}
	}

	//-----------------------------------------------------
	// showInvSlots
	//
	// Displays the players inventory on to screen.
	//-----------------------------------------------------
	void showInvSlots (int windowId){

		GUILayout.BeginArea (new Rect(5, 25, 95, 160));

		GUILayout.BeginHorizontal ();
		GUILayout.Button ("1: " + inventoryNameDictionary[0], GUILayout.Height (50));
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		GUILayout.Button ("2: " + inventoryNameDictionary[1], GUILayout.Height (50));
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		GUILayout.Button ("3: " + inventoryNameDictionary[2], GUILayout.Height (50));
		GUILayout.EndHorizontal ();

		GUILayout.EndArea ();

	}

	//-----------------------------------------------------
	// InvToggle()
	//
	// Show inventory on keypress.
	//-----------------------------------------------------
	void InvToggle(){
		if(Input.GetKeyDown("i") && isOpen == false){
			isOpen = true;
		}
		else if(Input.GetKeyDown("i") && isOpen != false){
			isOpen = false;
		}
	}

	//-----------------------------------------------------
	// Equip()
	//
	// If the selected item in the player's inventory is
	// not edible "equip" it.
	// TODO Show represented gameObject in game world.
	//-----------------------------------------------------
	void Equip(){
		if(Input.GetKeyDown ("1") && PlayerInventoryGUI.inventoryNameDictionary[0] != string.Empty){

			if(PlayerInventoryGUI.inventoryNameDictionary[0] == "Rock" || PlayerInventoryGUI.inventoryNameDictionary[0] == "Beehive"){
				// If the first slot is a Beehive create a beehive gameObject above the player.
				if(PlayerInventoryGUI.inventoryNameDictionary[0] == "Beehive"){



					if(clone != null){
					Debug.Log("Already Carrying Object");
					
					}
					else{
						clone = Instantiate(Beehive, playerTransform.position + new Vector3(0.0f,2.0f,0.0f), Quaternion.identity) as GameObject;
						Debug.Log("Instantiated");
						clone.transform.parent = playerTransform;
						Debug.Log("Beehive following player");
						PlayerInventoryGUI.inventoryNameDictionary[0] = string.Empty;
						Debug.Log ("Gone from inventory slot 1");
					}
				}
			}
		}

		else if(Input.GetKeyDown ("2") && PlayerInventoryGUI.inventoryNameDictionary[1] != string.Empty){

			if(PlayerInventoryGUI.inventoryNameDictionary[1] == "Rock" || PlayerInventoryGUI.inventoryNameDictionary[1] == "Beehive"){


				if(clone != null){
					Debug.Log("Already Carrying Object");
				}
				else{
					clone = Instantiate(Beehive, playerTransform.position + new Vector3(0.0f,2.0f,0.0f), Quaternion.identity) as GameObject;
					Debug.Log("Instantiated");
					clone.transform.parent = playerTransform;
					Debug.Log("Beehive following player");
					PlayerInventoryGUI.inventoryNameDictionary[1] = string.Empty;
					Debug.Log ("Gone from inventory slot 2");
				}
			}
		}
		
		else if(Input.GetKeyDown ("3") && PlayerInventoryGUI.inventoryNameDictionary[2] != string.Empty){
			if(PlayerInventoryGUI.inventoryNameDictionary[2] == "Rock" || PlayerInventoryGUI.inventoryNameDictionary[2] == "Beehive"){
							
				if(clone != null){
					Debug.Log("Already Carrying Object");

				}
				else{
					clone = Instantiate(Beehive, playerTransform.position + new Vector3(0.0f,2.0f,0.0f), Quaternion.identity) as GameObject;
					Debug.Log("Instantiated");
					clone.transform.parent = playerTransform;
					Debug.Log("Beehive following player");
					PlayerInventoryGUI.inventoryNameDictionary[2] = string.Empty;
					Debug.Log ("Gone from inventory slot 3");
				}
			}
		}
		
	}
}
