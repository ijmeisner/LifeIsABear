using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Attach to empty game object
public class LootBeehive : MonoBehaviour {
	private Rect inventoryWindowRect;
	private bool inventoryWindowShow;

	private BeehiveItems beehiveItems;

	//Changed to private
	private Dictionary<int, string> lootDictionary = new Dictionary<int, string>()
	{
		{0, string.Empty}
	};

	private Ray mouseRay;
	private RaycastHit rayHit;

	// Use this for initialization
	void Awake () {
		inventoryWindowRect = new Rect (300, 100, 175, 150);
		inventoryWindowShow = false;
	}
	
	// Update is called once per frame
	void Update () {

		mouseRay = Camera.main.ScreenPointToRay (Input.mousePosition);

		if (Input.GetMouseButton (1)) {		// Right click
			if(Physics.Raycast (mouseRay, out rayHit)){

			if(rayHit.collider.transform.tag == "Beehive"){

				//Selects the instance of the script attached to gameObject
				beehiveItems = rayHit.collider.gameObject.GetComponent<BeehiveItems>();

				//Initializes the lootDictionary in this one instance of the script.
				lootDictionary = beehiveItems.lootDictionary;

				if(PlayerInventoryGUI.isOpen){
					PlayerInventoryGUI.isOpen = false;
				}
		
				inventoryWindowShow = true;
			}
			}}
	
	}

	void OnGUI(){

		if (inventoryWindowShow) {
			inventoryWindowRect = GUI.Window(0, inventoryWindowRect, showInvSlots, "Beehive");
		}

	}


	void showInvSlots (int windowId){
	
		// Inventory button layout
		GUILayout.BeginArea (new Rect(0, 50, 200, 100));
		
		GUILayout.BeginHorizontal ();

		//The magic behind transferring an item from source to player inventory
		if(GUILayout.Button (lootDictionary[0], GUILayout.Height (50))){
			if(lootDictionary[0] != string.Empty){
			
				for(int i=0; i<3 && (lootDictionary[0] != string.Empty); i++){
					if(PlayerInventoryGUI.inventoryNameDictionary[i] == string.Empty){
						PlayerInventoryGUI.inventoryNameDictionary[i] = lootDictionary[0];
						lootDictionary[0] = string.Empty;
					}
				}
				inventoryWindowShow = false;

			}
			else{
				inventoryWindowShow = false;
			}
	
		}
	
		GUILayout.EndHorizontal ();

		GUILayout.EndArea ();	
		
	}	

}
