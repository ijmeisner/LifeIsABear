using UnityEngine;
using System.Collections;

public class ItemClass : MonoBehaviour {
	// Create new items here
	public ItemCreatorClass beehiveItem = new ItemCreatorClass(0,"Beehive", "expected...");

	// Use this for initialization
	void Awake () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public class ItemCreatorClass{
		public int id;
		public string name;
		public string description;
		// Constructor
		public ItemCreatorClass(int ide, string nam, string des)
		{
			id = ide;
			name = nam;
			description = des;
		}

		~ItemCreatorClass(){} // Destructor
	}
}
