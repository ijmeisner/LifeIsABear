using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Attach to empty game object
public class LootBerry : MonoBehaviour {
	private Rect inventoryWindowRect;
	private bool inventoryWindowShow;


	
	public Dictionary<int, string> lootDictionary = new Dictionary<int, string>()
	{
		{0, string.Empty}
	};

	private BerryItems berryItems;

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
			
			if(rayHit.collider.transform.tag == "Berry"){
				//aBerry = true;

				//Selects the instance of the script attached to gameObject
				berryItems = rayHit.collider.gameObject.GetComponent<BerryItems>();
				
				//Initializes the lootDictionary in this one instance of the script.
				lootDictionary = berryItems.lootDictionary;

				//Close the player's inventory before opening the loot window
				if(PlayerInventoryGUI.isOpen){
					PlayerInventoryGUI.isOpen = false;
				}
				inventoryWindowShow = true;
			}
			}
		}

	}
	
	void OnGUI(){
		
		if (inventoryWindowShow) {
			inventoryWindowRect = GUI.Window(0, inventoryWindowRect, showInvSlots, "Berries");
		}
		
	}
	
	
	void showInvSlots (int windowId){
		
		// Inventory button layout
		GUILayout.BeginArea (new Rect(0, 50, 175, 100));
		
		GUILayout.BeginHorizontal ();
		
		if(GUILayout.Button (lootDictionary[0], GUILayout.Height (50))){
			if(lootDictionary[0] != string.Empty){
				if(lootDictionary[0] != string.Empty){
					
					for(int i=0; i<3 && (lootDictionary[0] != string.Empty); i++){
						if(PlayerInventoryGUI.inventoryNameDictionary[i] == string.Empty){
							PlayerInventoryGUI.inventoryNameDictionary[i] = lootDictionary[0];
							lootDictionary[0] = string.Empty;
						}
					}
				
					inventoryWindowShow = false;
				}
			
		
			}
			else{
				inventoryWindowShow = false;
			}
		}
		
		GUILayout.EndHorizontal ();
		
		GUILayout.EndArea ();	
		
	}	
	
}
