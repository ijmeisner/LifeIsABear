using UnityEngine;
using System.Collections;

public class PlayerAttributes : MonoBehaviour {
	
	public float health;		// Player max health.
	public float curHealth;		// Current health of player.
	public float moveSpeed;		// Base movement speed of character
	public float strength;		// Base damage dealt by abilities.
	public float attRange;		// Max distance abilities can be used from the player.
	
	public float endurance;		// Base resource pool for sprinting, abilities, etc.
	public float curEndurance;	// Current endurance level.
	public float regeneration;	// Energy regeneration rate.

	public int curLevel;		// Level of player.
	public int maxLevel;		// Maximum player level.
	public int curExp;			// Base amount of experience on current level.
	public int maxExp;			// Experience needed to level
	public enum hungerLevel{happy, starving, chubby};	// Levels of hunger 
	
	
	void Start() {
		health = 10.0f;
		curHealth = health;
		endurance = 10.0f;

		curEndurance = endurance;
		regeneration = 1.0f;

		strength = 2.0f;
		attRange = 2.5f;

		curLevel = 1;
		maxLevel = 3;
		curExp = 1;
		maxExp = 10;
	}
	
	void Update() {

		LevelUp ();
		EnergyRegen();
		Health ();

		// Test for upper or lower boundaries.
		if (Input.GetKeyDown (KeyCode.H)) {
			curHealth -= 1;
		}
		// Test for upper or lower boundaries.
		if (Input.GetKeyDown (KeyCode.E)) {
			curEndurance -= 1;
		}
		// Test for upper or lower boundaries.
		if (Input.GetKeyDown (KeyCode.L)) {
			curExp += 1;
		}
	}

	//-----------------------------------------------------
	// Level Up
	// Increment level after curExp reaches maxExp in value
	// Reset curExp to 1
	// Increase base stats on level up.
	//-----------------------------------------------------
	void LevelUp(){
		if (curExp >= maxExp) {
			curExp = 1;
			if(curLevel < maxLevel){
				curLevel++;
				strength += curLevel;
				health += 5;
				curHealth = health;
				endurance += 2;
				curEndurance = endurance;
			}
		}
	}

	//-----------------------------------------------------
	// Energy regeneration
	// curEndurance values are always greater than or equal
	// to 0 or less than or equal to endurance.
	//-----------------------------------------------------
	void EnergyRegen(){
		if (curEndurance < endurance) {
			curEndurance += regeneration * Time.deltaTime;
			if (curEndurance > endurance) {
				curEndurance = endurance;
			}
			if (curEndurance < 0) {
				curEndurance = 0;
			}	
		}
	}

	//-----------------------------------------------------
	// Health
	// Checks if curHealth is within 0 and the max
	// health limit.
	//-----------------------------------------------------
	void Health(){
		if (curHealth > health) {
			curHealth = health;
		}
		if (curHealth < 0) {
			curHealth = 0;
		}
	}


	//-----------------------------------------------------
	// Displays player statistics on ccreen.
	//-----------------------------------------------------
	void OnGUI()
	{
		GUI.Box(new Rect(20, 30, 200, 20), "Level: " + curLevel);
		GUI.Box(new Rect(20, 50, 200, 20), "Exp: " + curExp + " / " + maxExp);
		GUI.Box(new Rect(20, 70, 200, 20), "Health: " + curHealth + " / " + health);
		GUI.Box(new Rect(20, 90, 200, 20), "Stam: " + curEndurance + " / " + endurance);
		GUI.Box(new Rect(20, 110, 200, 20), "Strength: " + strength);
		
	}
	
}