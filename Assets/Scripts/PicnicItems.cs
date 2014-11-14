using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Attach to picnic basket prefab
public class PicnicItems : MonoBehaviour {
	//ItemClass itemObject = new ItemClass();
	public Dictionary<int, string> lootDictionary = new Dictionary<int, string>()
	{
		{0, string.Empty}
	};
	
	// Use this for initialization
	void Start () {
		
		lootDictionary [0] = "Basket";
		
	}

}
