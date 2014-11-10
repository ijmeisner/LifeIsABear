using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemLoot : MonoBehaviour {
	private Rect inventoryWindowRect = new Rect (300, 100, 200, 200);
	private bool inventoryWindowShow = false;

	private Dictionary<int, string> lootDictionary = new Dictionary<int, string>()
	{
		{0, string.Empty},
		{1, string.Empty},
		{2, string.Empty}
	};

	ItemClass itemObject = new ItemClass();

	private Ray mouseRay;
	private RaycastHit rayHit;

	// Use this for initialization
	void Start () {

		//Display Dictionary
		lootDictionary [0] = itemObject.beehiveItem.name;
	
	}
	
	// Update is called once per frame
	void Update () {

		mouseRay = Camera.main.ScreenPointToRay (Input.mousePosition);

		if (Input.GetMouseButton (1)) {		// Right click
			Physics.Raycast (mouseRay, out rayHit);
			if(rayHit.collider.transform.tag == "LootableItem"){
				inventoryWindowShow = true;
			}
		}

		//Close loot window
		if(Input.GetKeyDown ("l")){
			inventoryWindowShow = false;
		}

	
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

		if(GUILayout.Button (lootDictionary[0], GUILayout.Height (50))){
			if(lootDictionary[0] != string.Empty){
				PlayerInventoryGUI.inventoryNameDictionary[0] = lootDictionary[0];
				lootDictionary[0] = string.Empty;
			}
		}
	
		GUILayout.EndHorizontal ();

		GUILayout.EndArea ();	
		
	}	

}
