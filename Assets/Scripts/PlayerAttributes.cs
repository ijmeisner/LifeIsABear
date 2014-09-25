using UnityEngine;
using System.Collections;

public class PlayerAttributes : MonoBehaviour {

	public float health = 10.0f;		// Player max health.
	public float curHealth;				// Current health of player.
	public float moveSpeed;				// Base movement speed of character
	public float strength = 2.0f;		// Base damage dealt by abilities.
	public float attRange = 2.5f;		// Max distance abilities can be used from the player.

	public float endurance = 5.0f;		// Base resource pool for sprinting, abilities, etc.
	public float curEndurance;			// Current endurance level.
	public float regeneration = 0.2f;	// Energy regeneration rate.
	public int[] inventory = new int[5];// Max number of items player can have in inventory.
	public int curLevel = 1;			// Level of player.
	public int maxLevel;				// Maximum player level.
	public int curExp = 1;				// Base amount of experience on current level.
	private int maxExp = 100;			// Experience needed to level
	public bool hungry;					//


	void Start() {
		curHealth = health;
		curEndurance = endurance;
	}

	void Update() {
		// Increment level after curExp reaches maxExp in value
		// Reset curExp to 1
		if (curExp >= maxExp) {
			curExp = 1;
			curLevel++;
		}

		// Energy regeneration
		// curEndurance values are always greater than or equal
		// to 0 or less than or equal to endurance.
		if (curEndurance < endurance) {
			curEndurance += regeneration * Time.deltaTime;
			if (curEndurance > endurance) {
				curEndurance = endurance;
			}
			if (curEndurance < 0) {
				curEndurance = 0;
			}	
		}

		// Boundaries for health values
		if (curHealth > health) {
			curHealth = health;
		}
		if (curHealth < 0) {
			curHealth = 0;
		}	

		// Test for upper or lower boundaries.
		if (Input.GetKeyDown (KeyCode.E)) {
			curHealth -= 1;
		}

		// Test for upper or lower boundaries.
		if (Input.GetKeyDown (KeyCode.F)) {
			curEndurance -= 1;
		}

		// Test for upper or lower boundaries.
		if (Input.GetKeyDown (KeyCode.G)) {
			curExp += 1;
		}
	}

	void OnGUI()
	{
		GUI.Box(new Rect(20, 50, 200, 20), curHealth + " / " + health);
		GUI.Box(new Rect(20, 90, 200, 20), "Level: " + curHealth);
		
	}

}
