using UnityEngine;
using System.Collections;

// Does not need to be attached in Unity
public class ItemClass {
	// Create new items here
	// TODO initialize canPickUp and canThrow values to items
	public ItemCreatorClass beehiveItem = new ItemCreatorClass(0,"Beehive", false, "OUCH!"/*, true, true*/);
	public ItemCreatorClass picnicItem = new ItemCreatorClass(1, "Basket", true, "Yummy!"/*,false, false*/);
	public ItemCreatorClass berryItem = new ItemCreatorClass(2, "Berries", true, "Juicy!"/*false, false*/);
	public ItemCreatorClass rockItem = new ItemCreatorClass (3, "Rock", false, "Hmmm")/*, true, true*/;

	public class ItemCreatorClass{
		public int id;
		public string name;
		public bool edible;
		public string description;
		//public bool canPickUp;
		//public bool canThrow;

		public ItemCreatorClass(int ide, string nam, bool eat, string des)
		{
			id = ide;
			name = nam;
			edible = eat;
			description = des;
		}
	}

}
